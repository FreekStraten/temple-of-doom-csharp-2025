using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Models;

namespace TempleOfDoom.BusinessLogic.Interfaces
{
    public interface IRoomTransitionService
    {
        bool TryTransition(Room currentRoom, Player player, Direction direction, out Room nextRoom);
    }
}
