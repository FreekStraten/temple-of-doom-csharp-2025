using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class ClosingGateDoorDecorator : DoorDecorator
    {
        private bool _hasClosed = false;

        public ClosingGateDoorDecorator(IDoor wrappedDoor)
            : base(wrappedDoor)
        {
        }

        public override bool IsOpen(Player player, Room currentRoom)
        {
            // As soon as it's passed once, we close forever
            // It's "open" only if not closed YET and the base chain is also open
            return !_hasClosed && base.IsOpen(player, currentRoom);
        }

        public override char GetRepresentation(bool isHorizontal)
        {
            return _hasClosed ? 'n' : ' ';
        }

        public override ConsoleColor GetColor()
        {
            return _hasClosed ? ConsoleColor.DarkMagenta : ConsoleColor.White;
        }

        public override void NotifyStateChange()
        {
            // after passing the door, we close it
            _hasClosed = true;
            // optionally call base
            base.NotifyStateChange();
        }
    }
}
