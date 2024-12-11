using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Models;

namespace TempleOfDoom.BusinessLogic.Interfaces
{
    public interface IDoorService
    {
        bool CanPassThroughDoor(Player player, Room room, Direction direction);
        void AfterPassingDoor(Room room, Direction direction);
    }

}
