using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Models;

namespace TempleOfDoom.BusinessLogic.Interfaces
{
    public interface IPlayerMovementController
    {
        /// <summary>
        /// Attempt to move the player in the given direction inside the current room.
        /// Returns true if moved inside the room, false if not.
        /// If movement is not possible inside the room, might trigger room transition.
        /// </summary>
        bool TryMovePlayer(Player player, Room currentRoom, Direction direction);
    }

}
