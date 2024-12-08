using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class OpenOnOddDoor : IDoor
    {
        public char GetRepresentation(bool isHorizontal)
            => isHorizontal ? '=' : '|';
        public ConsoleColor GetColor() => ConsoleColor.Magenta;
        public bool IsOpen(Player player, Room currentRoom)
        {
            // Placeholder: always closed
            return false;
        }
        public void NotifyStateChange() { }
    }
}
