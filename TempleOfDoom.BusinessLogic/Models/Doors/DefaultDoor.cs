using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class DefaultDoor : IDoor
    {
        public char GetRepresentation(bool isHorizontal) => ' ';
        public ConsoleColor GetColor() => ConsoleColor.White;
        public bool IsOpen(Player player, Room currentRoom) => true;
        public void NotifyStateChange() { /* No op for default door */ }
    }
}
