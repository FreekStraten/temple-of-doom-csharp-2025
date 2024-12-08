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
        private bool _isOpen = false; // default state

        public char GetRepresentation(bool isHorizontal)
            => isHorizontal ? '=' : '|'; // same as colored, but logic differs

        public ConsoleColor GetColor() => ConsoleColor.Yellow;

        public bool IsOpen(Player player, Room currentRoom) => _isOpen;

        public void NotifyStateChange()
        {
            // Flip the state on pressure plate activation (to be implemented later)
            _isOpen = !_isOpen;
        }
    }
}
