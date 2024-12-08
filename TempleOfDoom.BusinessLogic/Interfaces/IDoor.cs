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
        // Returns a character representation depending on orientation
        char GetRepresentation(bool isHorizontal);

        // Returns the color used to render this door
        ConsoleColor GetColor();

        // Skeleton methods for logic:
        bool IsOpen(Player player, Room currentRoom);
        // We won't implement logic yet, just return true/false placeholders.

        // Some doors might need to be toggled externally (e.g., pressure plates)
        void NotifyStateChange();
    }
}
