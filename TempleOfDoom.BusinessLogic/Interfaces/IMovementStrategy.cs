using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Interfaces
{
    public interface IMovementStrategy
    {
        /// <summary>
        /// Calculates the next position for the player given a direction.
        /// </summary>
        Coordinates GetNextPosition(Player player, Room currentRoom, Direction direction);
    }
}
