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
        private bool _hasClosed = false;

        public char GetRepresentation(bool isHorizontal) => 'n';
        public ConsoleColor GetColor() => ConsoleColor.DarkMagenta;

        public bool IsOpen(Player player, Room currentRoom)
        {
            return !_hasClosed; // open until it closes once
        }

        public void NotifyStateChange()
        {
            // After passing once, door closes forever
            _hasClosed = true;
        }
    }
}
