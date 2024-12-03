using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic
{
    public class GameService
    {
        private Room _currentRoom;
        private Player _player;

        public GameService(Room currentRoom, Player player)
        {
            _currentRoom = currentRoom;
            _player = player;
        }

        public void HandlePlayerMovement(Direction direction)
        {
            Coordinates movement = direction switch
            {
                Direction.North => new Coordinates(0, -1),
                Direction.South => new Coordinates(0, 1),
                Direction.West => new Coordinates(-1, 0),
                Direction.East => new Coordinates(1, 0),
                _ => new Coordinates(0, 0)
            };

            Coordinates newPosition = _player.Position + movement;

            // Check if the new position is within bounds
            if (newPosition.X < 0 || newPosition.X >= _currentRoom.Width ||
                newPosition.Y < 0 || newPosition.Y >= _currentRoom.Height)
            {
                return; // Out of bounds
            }

            // Check if the tile at the new position is walkable
            if (_currentRoom.GetTileAt(newPosition).IsWalkable)
            {
                _player.Position = newPosition;
            }
        }


        // Other game logic methods can be added here
    }
}
