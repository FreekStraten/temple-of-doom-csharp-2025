using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class ClosingGateDoor : IDoor
    {
        public char GetRepresentation(bool isHorizontal)
            => isHorizontal ? '=' : '|';
        public ConsoleColor GetColor() => ConsoleColor.DarkMagenta;
        public bool IsOpen(Player player, Room currentRoom)
        {
            // Placeholder logic
            return true;
        }
        public void NotifyStateChange() { }
    }
}
