using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class PressurePlateDoor : IDoor
    {
        public char GetRepresentation(bool isHorizontal) => '⊥';
        public ConsoleColor GetColor() => ConsoleColor.Cyan;
        public bool IsOpen(Player player, Room currentRoom)
        {
            // Placeholder: maybe always open for now
            return true;
        }

        public void NotifyStateChange() { /* Not used yet */ }
    }
}
