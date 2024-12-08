using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Helpers;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Strategies
{
    public class DefaultMovementStrategy : IMovementStrategy
    {
        public Coordinates GetNextPosition(Player player, Room currentRoom, Direction direction)
        {
            Coordinates movement = DirectionHelper.ToCoordinates(direction);
            Coordinates newPosition = player.Position + movement;

            // Boundary checks and collision checks can be done here or delegated
            // to another service if needed.

            return newPosition;
        }
    }
}
