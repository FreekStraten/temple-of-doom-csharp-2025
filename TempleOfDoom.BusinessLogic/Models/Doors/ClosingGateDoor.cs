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

        public char GetRepresentation(bool isHorizontal)
        {
            // If the door has not closed yet (has not been passed through), 
            // it should appear as a normal, empty door passage " " 
            // to trick the player into thinking it's a normal open connection.
            // Once closed (after passing), show 'n'.
            return _hasClosed ? 'n' : ' ';
        }

        public ConsoleColor GetColor()
        {
            // When not yet closed, appear as a default (white) passage.
            // After closing, use the distinctive color.
            return _hasClosed ? ConsoleColor.DarkMagenta : ConsoleColor.White;
        }

        public bool IsOpen(Player player, Room currentRoom)
        {
            // It's open until player passes through once; after that, it closes forever.
            return !_hasClosed;
        }

        public void NotifyStateChange()
        {
            // After passing once, door closes forever.
            // This is typically called after passing through it the first time.
            _hasClosed = true;
        }
    }

}
