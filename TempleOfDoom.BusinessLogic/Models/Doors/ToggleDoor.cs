using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class ToggleDoor : IDoor
    {
        private bool _isOpen = false; // default state closed

        public char GetRepresentation(bool isHorizontal) => '\u22A5';

        public ConsoleColor GetColor() => ConsoleColor.Yellow;

        public bool IsOpen(Player player, Room currentRoom) => _isOpen;

        public void NotifyStateChange()
        {
            _isOpen = !_isOpen;
        }
    }
}
