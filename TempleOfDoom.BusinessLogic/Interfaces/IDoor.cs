using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Models;

namespace TempleOfDoom.BusinessLogic.Interfaces
{
    public interface IDoor
    {
        char GetRepresentation(bool isHorizontal);

        ConsoleColor GetColor();

        bool IsOpen(Player player, Room currentRoom);

        // Some doors might need to be toggled externally (e.g., pressure plates)
        void NotifyStateChange();
    }
}
