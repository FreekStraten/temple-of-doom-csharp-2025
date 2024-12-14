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
        /// Returns true if moved or transitioned, false otherwise.
        /// If room transition occurs, the new room is returned in newCurrentRoom.
        /// </summary>
        bool TryMovePlayer(Player player, Room currentRoom, Direction direction, out Room newCurrentRoom);
    }
}

