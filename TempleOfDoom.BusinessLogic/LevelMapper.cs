using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CODE_TempleOfDoom_DownloadableContent;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Factories;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Models.Doors;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Struct;
using TempleOfDoom.DataAccess;

namespace TempleOfDoom.BusinessLogic.Mappers
{
    public static class LevelMapper
    {

            private static Dictionary<(int roomId, int x, int y), (int roomId, int x, int y)> _ladderLookup 
        = new Dictionary<(int, int, int), (int, int, int)>();

        public static Room MapRoomDtoToRoom(RoomDto roomDto, IItemFactory itemFactory)
        {
            var room = new Room(roomDto.Width, roomDto.Height)
            {
                Id = roomDto.Id,
                Type = roomDto.Type
            };
            room.GenerateLayout();

            // Place items
            if (roomDto.Items != null)
            {
                foreach (var itemDto in roomDto.Items)
                {
                    var item = itemFactory.CreateItem(itemDto);
                    room.PlaceItem(new Coordinates(itemDto.X, itemDto.Y), item);
                }
            }

            // Create real Enemy objects from the DLL
            if (roomDto.Enemies != null)
            {
                foreach (var enemyDto in roomDto.Enemies)
                {
                    Enemy enemy = CreateEnemyFromDto(enemyDto);

                    // Set the correct FieldAdapter
                    enemy.CurrentField = room.FieldAdapters[enemyDto.Y, enemyDto.X];
                    room.FieldAdapters[enemyDto.Y, enemyDto.X].Item = enemy;

                    // Subscribe to OnDeath event
                    enemy.OnDeath += (sender, e) =>
                    {
                        room.Enemies.Remove(enemy);
                        // If the enemy’s field is still pointing to this enemy, clear it
                        if (enemy.CurrentField != null)
                        {
                            enemy.CurrentField.Item = null;
                        }
                    };

                    room.Enemies.Add(enemy);
                }
            }

            return room;
        }

        private static Enemy CreateEnemyFromDto(EnemyDto dto)
        {
            // For example, all enemies get 3 lives by default (or read from JSON).
            int initialLives = 1;

            switch (dto.Type.ToLower())
            {
                case "horizontal":
                    return new HorizontallyMovingEnemy(
                        initialLives,
                        dto.X,
                        dto.Y,
                        dto.MinX,
                        dto.MaxX
                    );
                case "vertical":
                    return new VerticallyMovingEnemy(
                        initialLives,
                        dto.X,
                        dto.Y,
                        dto.MinY,
                        dto.MaxY
                    );
                default:
                    throw new ArgumentException($"Unknown enemy type: {dto.Type}");
            }
        }

        public static Player MapPlayerDtoToPlayer(PlayerDto playerDto, IGameStateManager gameStateManager)
        {
            return new Player(playerDto.StartX, playerDto.StartY, playerDto.Lives, gameStateManager);
        }


        public static void ApplyConnectionsToRooms(List<ConnectionDto> connections, Dictionary<int, Room> rooms)
        {
            foreach (var connection in connections)
            {
                // --------------------------------------
                // Existing DOOR logic (NORTH, SOUTH, etc.)
                // --------------------------------------
                if (connection.NORTH.HasValue && connection.SOUTH.HasValue)
                {
                    IDoor door = DoorFactory.CreateCompositeDoor(connection.Doors);
                    CreateDoorTileForRoom(rooms[connection.NORTH.Value], Direction.South, door);
                    CreateDoorTileForRoom(rooms[connection.SOUTH.Value], Direction.North, door);
                }
                if (connection.EAST.HasValue && connection.WEST.HasValue)
                {
                    IDoor door = DoorFactory.CreateCompositeDoor(connection.Doors);
                    CreateDoorTileForRoom(rooms[connection.EAST.Value], Direction.West, door);
                    CreateDoorTileForRoom(rooms[connection.WEST.Value], Direction.East, door);
                }

                // --------------------------------------
                // NEW LADDER LOGIC
                // --------------------------------------
                if (connection.UPPER.HasValue && connection.LOWER.HasValue && connection.Ladder != null)
                {
                    // Identify which rooms are upper vs lower
                    var upperRoom = rooms[connection.UPPER.Value];
                    var lowerRoom = rooms[connection.LOWER.Value];

                    // Ladder positions within each room
                    var ladderUpX = connection.Ladder.upperX;
                    var ladderUpY = connection.Ladder.upperY;
                    var ladderDownX = connection.Ladder.lowerX;
                    var ladderDownY = connection.Ladder.lowerY;

                    // 1) Create a LadderTile in the upper room that leads DOWN
                    //    That means it teleports us to the "lower" room at (ladderDownX, ladderDownY).
                    var ladderTileDown = new LadderTile(
                        connectedRoomId: connection.LOWER.Value,
                        targetCoordinates: new Coordinates(ladderDownX, ladderDownY)
                    );
                    upperRoom.Layout[ladderUpY, ladderUpX] = ladderTileDown;

                    // 2) Create a LadderTile in the lower room that leads UP
                    //    That means it teleports us to the "upper" room at (ladderUpX, ladderUpY).
                    var ladderTileUp = new LadderTile(
                        connectedRoomId: connection.UPPER.Value,
                        targetCoordinates: new Coordinates(ladderUpX, ladderUpY)
                    );
                    lowerRoom.Layout[ladderDownY, ladderDownX] = ladderTileUp;
                }
            }
        }



        private static void CreateDoorTileForRoom(Room room, Direction direction, IDoor door)
        {
            bool isHorizontal = (direction == Direction.North || direction == Direction.South);
            Coordinates doorPosition = GetDoorPosition(room, direction);
            room.Layout[doorPosition.Y, doorPosition.X] = new DoorTile(door, isHorizontal);

            // Register if needed
            if (door is ToggleDoorDecorator || door is DoorDecorator)
            {
                room.RegisterDoor(door);
            }
        }

        private static Coordinates GetDoorPosition(Room room, Direction direction)
        {
            int doorX = 0, doorY = 0;
            switch (direction)
            {
                case Direction.North: doorX = room.Width / 2; doorY = 0; break;
                case Direction.South: doorX = room.Width / 2; doorY = room.Height - 1; break;
                case Direction.West: doorX = 0; doorY = room.Height / 2; break;
                case Direction.East: doorX = room.Width - 1; doorY = room.Height / 2; break;
            }
            return new Coordinates(doorX, doorY);
        }

        public static Dictionary<int, Dictionary<Direction, int>> CreateRoomConnectionMap(List<ConnectionDto> connections)
        {
            var map = new Dictionary<int, Dictionary<Direction, int>>();
            foreach (var conn in connections)
            {
                if (conn.NORTH.HasValue && conn.SOUTH.HasValue)
                {
                    AddConnection(map, conn.NORTH.Value, Direction.South, conn.SOUTH.Value);
                    AddConnection(map, conn.SOUTH.Value, Direction.North, conn.NORTH.Value);
                }
                if (conn.EAST.HasValue && conn.WEST.HasValue)
                {
                    AddConnection(map, conn.EAST.Value, Direction.West, conn.WEST.Value);
                    AddConnection(map, conn.WEST.Value, Direction.East, conn.EAST.Value);
                }
            }
            return map;
        }

        private static void AddConnection(Dictionary<int, Dictionary<Direction, int>> map, int fromRoomId, Direction dir, int toRoomId)
        {
            if (!map.ContainsKey(fromRoomId))
                map[fromRoomId] = new Dictionary<Direction, int>();
            map[fromRoomId][dir] = toRoomId;
        }
    }
}
