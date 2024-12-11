using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Services
{
    public class RoomTransitionService : IRoomTransitionService
    {
        private readonly Dictionary<int, Dictionary<Direction, int>> _roomConnections;
        private readonly IDoorService _doorService;

        public RoomTransitionService(Dictionary<int, Dictionary<Direction, int>> roomConnections, IDoorService doorService)
        {
            _roomConnections = roomConnections;
            _doorService = doorService;
        }

        public bool TryTransition(Room currentRoom, Player player, Direction direction, out Room nextRoom)
        {
            nextRoom = null;
            if (!_roomConnections.TryGetValue(currentRoom.Id, out var connForRoom))
                return false;

            if (!connForRoom.TryGetValue(direction, out int nextRoomId))
                return false;

            // Since door was deemed passable before calling transition, we close gates if needed:
            _doorService.AfterPassingDoor(currentRoom, direction);

            // Calculate entry position
            nextRoom = GetNextRoom(nextRoomId);
            var entryPos = GetEntryPositionInNextRoom(nextRoom, OppositeDirection(direction));
            player.UpdatePosition(entryPos);

            return true;
        }

        private Room GetNextRoom(int roomId)
        {
            // This should be retrieved from a room repository or passed in constructor
            // For simplicity, assume we have access to all rooms:
            return GameService.RoomsById[roomId]; // If you keep a static dictionary or injection
        }

        private Coordinates GetEntryPositionInNextRoom(Room nextRoom, Direction comingFromDirection)
        {
            return comingFromDirection switch
            {
                Direction.North => new Coordinates(nextRoom.Width / 2, 0),
                Direction.South => new Coordinates(nextRoom.Width / 2, nextRoom.Height - 1),
                Direction.West => new Coordinates(0, nextRoom.Height / 2),
                Direction.East => new Coordinates(nextRoom.Width - 1, nextRoom.Height / 2),
                _ => new Coordinates(1, 1)
            };
        }

        private Direction OppositeDirection(Direction direction)
        {
            return direction switch
            {
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.East => Direction.West,
                Direction.West => Direction.East,
                _ => direction
            };
        }
    }

}
