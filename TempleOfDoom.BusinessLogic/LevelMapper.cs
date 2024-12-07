using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.BusinessLogic.Mappers
{
    using TempleOfDoom.BusinessLogic.Enum;
    using TempleOfDoom.BusinessLogic.Models;
    using TempleOfDoom.BusinessLogic.Models.Tile;
    using TempleOfDoom.BusinessLogic.Struct;
    using TempleOfDoom.DataAccess;

    public static class LevelMapper
    {
        public static Room MapRoomDtoToRoom(RoomDto roomDto)
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
                    var item = ItemFactory.CreateItem(itemDto);
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
                if (connection.NORTH.HasValue) PlaceDoorTile(rooms[connection.NORTH.Value], Direction.South);
                if (connection.SOUTH.HasValue) PlaceDoorTile(rooms[connection.SOUTH.Value], Direction.North);
                if (connection.EAST.HasValue) PlaceDoorTile(rooms[connection.EAST.Value], Direction.West);
                if (connection.WEST.HasValue) PlaceDoorTile(rooms[connection.WEST.Value], Direction.East);
            }
        }

        private static void PlaceDoorTile(Room room, Direction direction)
        {
            int doorX = 0, doorY = 0;
            switch (direction)
            {
                case Direction.North: doorX = room.Width / 2; doorY = 0; break;
                case Direction.South: doorX = room.Width / 2; doorY = room.Height - 1; break;
                case Direction.West: doorX = 0; doorY = room.Height / 2; break;
                case Direction.East: doorX = room.Width - 1; doorY = room.Height / 2; break;
            }
            room.Layout[doorY, doorX] = new DoorTile();
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
