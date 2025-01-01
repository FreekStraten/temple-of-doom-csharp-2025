using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class ToggleDoorDecorator : DoorDecorator
    {
        private bool _isOpen = false; // default closed

        public ToggleDoorDecorator(IDoor wrappedDoor)
            : base(wrappedDoor)
        { }

        public override char GetRepresentation(bool isHorizontal)
        {
            // special toggle representation
            return '\u22A5'; // or any symbol you like
        }

        public override ConsoleColor GetColor()
        {
            // highlight toggles in yellow, for example
            return ConsoleColor.Yellow;
        }

        public override bool IsOpen(Player player, Room currentRoom)
        {
            // Must also be open in the chain above
            // Return true if BOTH chain is open and _isOpen is true
            return base.IsOpen(player, currentRoom) && _isOpen;
        }

        public override void NotifyStateChange()
        {
            // Flip state
            _isOpen = !_isOpen;
            // We also call base.NotifyStateChange if we want to let the rest 
            // of the chain know, or do something else. 
            base.NotifyStateChange();
        }
    }

}
