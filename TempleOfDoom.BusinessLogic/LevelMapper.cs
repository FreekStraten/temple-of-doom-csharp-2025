using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.BusinessLogic.Mappers
{
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
                    // North's south wall is a door:
                    CreateDoorTileForRoom(rooms[connection.NORTH.Value], Direction.South, connection.Doors);
                    // South's north wall is a door:
                    CreateDoorTileForRoom(rooms[connection.SOUTH.Value], Direction.North, connection.Doors);
                }

                // East/West
                if (connection.EAST.HasValue && connection.WEST.HasValue)
                {
                    // East's west wall is a door:
                    CreateDoorTileForRoom(rooms[connection.EAST.Value], Direction.West, connection.Doors);
                    // West's east wall is a door:
                    CreateDoorTileForRoom(rooms[connection.WEST.Value], Direction.East, connection.Doors);
                }

                // In the future, handle UPPER/LOWER or portal similarly
            }
        }

        private static void CreateDoorTileForRoom(Room room, Direction direction, List<DoorDto> doorDtos)
        {
            // Determine orientation based on direction
            bool isHorizontal = (direction == Direction.North || direction == Direction.South);

            // If there are no doors defined, it's a default door
            IDoor door;
            if (doorDtos == null || doorDtos.Count == 0)
            {
                door = new DefaultDoor();
            }
            else if (doorDtos.Count == 1)
            {
                door = DoorFactory.CreateDoor(doorDtos[0]);
            }
            else
            {
                // If multiple door conditions apply, we could combine them via a Decorator pattern.
                // For now, just take the first one:
                // TODO: Implement a door decorator to combine multiple door conditions.
                door = DoorFactory.CreateDoor(doorDtos[0]);
            }

            Coordinates doorPosition = GetDoorPosition(room, direction);
            room.Layout[doorPosition.Y, doorPosition.X] = new DoorTile(door, isHorizontal);
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
