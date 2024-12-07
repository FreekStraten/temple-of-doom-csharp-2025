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

            // In the future, we can also place items or handle special logic here
            return room;
        }


        /*
        public static Item MapItemDtoToItem(ItemDto itemDto)
        {
            return new Item
            {
                // Map ItemDto properties here
                X = itemDto.X,
                Y = itemDto.Y
            };
        }
        */

        public static Player MapPlayerDtoToPlayer(PlayerDto playerDto)
        {
            Console.WriteLine($"PlayerDto: StartX: {playerDto.StartX}, StartY: {playerDto.StartY}, Lives: {playerDto.Lives}");
            return new Player(playerDto.StartX, playerDto.StartY, playerDto.Lives);
        }

        public static void ApplyConnectionsToRooms(List<ConnectionDto> connections, Dictionary<int, Room> rooms)
        {
            // Loop through each connection. Each connectionDto describes how rooms are connected.
            foreach (var connection in connections)
            {
                // For each direction in the connection (NORTH, SOUTH, EAST, WEST), 
                // we need to place a door tile in the appropriate room on the correct wall.

                // If NORTH is present, place a door in that room's south wall or vice versa.
                // Actually, each connection line typically describes a line between rooms. 
                // For now, let's just place door tiles in both connected rooms at their facing walls.

                // NORTH connection: The room you can enter from the south side of that room.
                if (connection.NORTH.HasValue)
                {
                    Room northRoom = rooms[connection.NORTH.Value];
                    // Door to the south boundary of northRoom (top side connecting downward)
                    PlaceDoorTile(northRoom, Direction.South);
                }

                if (connection.SOUTH.HasValue)
                {
                    Room southRoom = rooms[connection.SOUTH.Value];
                    // Door to the north boundary of southRoom (bottom side connecting upward)
                    PlaceDoorTile(southRoom, Direction.North);
                }

                if (connection.EAST.HasValue)
                {
                    Room eastRoom = rooms[connection.EAST.Value];
                    // Door to the west boundary of eastRoom (right side connecting leftward)
                    PlaceDoorTile(eastRoom, Direction.West);
                }

                if (connection.WEST.HasValue)
                {
                    Room westRoom = rooms[connection.WEST.Value];
                    // Door to the east boundary of westRoom (left side connecting rightward)
                    PlaceDoorTile(westRoom, Direction.East);
                }
            }
        }

        private static void PlaceDoorTile(Room room, Direction direction)
        {
            // Determine the coordinate in the room where the door should be placed.
            // Doors are always in the middle of a wall.
            int doorX = 0;
            int doorY = 0;

            switch (direction)
            {
                case Direction.North:
                    doorX = room.Width / 2;
                    doorY = 0; // top wall
                    break;
                case Direction.South:
                    doorX = room.Width / 2;
                    doorY = room.Height - 1; // bottom wall
                    break;
                case Direction.West:
                    doorX = 0;
                    doorY = room.Height / 2;
                    break;
                case Direction.East:
                    doorX = room.Width - 1;
                    doorY = room.Height / 2;
                    break;
            }

            // Replace the tile at these coordinates with a DoorTile
            room.Layout[doorY, doorX] = new DoorTile();
        }
    }
}
