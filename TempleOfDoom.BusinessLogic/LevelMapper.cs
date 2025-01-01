using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.BusinessLogic.Mappers
{
    using TempleOfDoom.BusinessLogic.Decorators;
    using TempleOfDoom.BusinessLogic.Enum;
    using TempleOfDoom.BusinessLogic.Factories;
    using TempleOfDoom.BusinessLogic.Interfaces;
    using TempleOfDoom.BusinessLogic.Models;
    using TempleOfDoom.BusinessLogic.Models.Doors;
    using TempleOfDoom.BusinessLogic.Models.Tile;
    using TempleOfDoom.BusinessLogic.Struct;
    using TempleOfDoom.DataAccess;

    public static class LevelMapper
    {
        public static Room MapRoomDtoToRoom(RoomDto roomDto, IItemFactory itemFactory)
        {
            var room = new Room(roomDto.Width, roomDto.Height)
            {
                Id = roomDto.Id,
                Type = roomDto.Type
            };
            room.GenerateLayout();

            if (roomDto.Items != null)
            {
                foreach (var itemDto in roomDto.Items)
                {
                    var item = itemFactory.CreateItem(itemDto);
                    room.PlaceItem(new Coordinates(itemDto.X, itemDto.Y), item);
                }
            }

            return room;
        }

        public static Player MapPlayerDtoToPlayer(PlayerDto playerDto)
        {
            return new Player(playerDto.StartX, playerDto.StartY, playerDto.Lives);
        }

        public static void ApplyConnectionsToRooms(List<ConnectionDto> connections, Dictionary<int, Room> rooms)
        {
            foreach (var connection in connections)
            {
                // Apply to north/south
                if (connection.NORTH.HasValue && connection.SOUTH.HasValue)
                {
                    IDoor door = DoorFactory.CreateCompositeDoor(connection.Doors);
                    CreateDoorTileForRoom(rooms[connection.NORTH.Value], Direction.South, door);
                    CreateDoorTileForRoom(rooms[connection.SOUTH.Value], Direction.North, door);
                }

                // East/West
                if (connection.EAST.HasValue && connection.WEST.HasValue)
                {
                    IDoor door = DoorFactory.CreateCompositeDoor(connection.Doors);
                    CreateDoorTileForRoom(rooms[connection.EAST.Value], Direction.West, door);
                    CreateDoorTileForRoom(rooms[connection.WEST.Value], Direction.East, door);
                }

                // If in part 2 you have UPPER/LOWER or portals, you'd handle them here similarly.
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
