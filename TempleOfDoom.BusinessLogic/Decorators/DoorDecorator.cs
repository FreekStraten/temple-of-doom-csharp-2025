using System;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;

namespace TempleOfDoom.BusinessLogic.Decorators
{
    public abstract class DoorDecorator : IDoor
    {
        protected readonly IDoor _wrappedDoor;

        protected DoorDecorator(IDoor wrappedDoor)
        {
            _wrappedDoor = wrappedDoor;
        }

        // Public property to access the wrapped door
        public IDoor WrappedDoor => _wrappedDoor;

        public virtual char GetRepresentation(bool isHorizontal)
        {
            return _wrappedDoor.GetRepresentation(isHorizontal);
        }

        public virtual ConsoleColor GetColor()
        {
            return _wrappedDoor.GetColor();
        }

        public virtual bool IsOpen(Player player, Room currentRoom)
        {
            return _wrappedDoor.IsOpen(player, currentRoom);
        }

        public virtual void NotifyStateChange()
        {
            _wrappedDoor.NotifyStateChange();
        }
    }
}
