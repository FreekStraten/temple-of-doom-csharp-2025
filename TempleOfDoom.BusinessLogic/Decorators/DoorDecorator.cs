using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;

namespace TempleOfDoom.BusinessLogic.Decorators
{
    public class DoorDecorator : IDoor
    {
        private readonly IDoor _primary;
        private readonly IDoor _secondary;

        public DoorDecorator(IDoor primary, IDoor secondary)
        {
            _primary = primary;
            _secondary = secondary;
        }

        public IDoor PrimaryDoor => _primary;
        public IDoor SecondaryDoor => _secondary;

        public char GetRepresentation(bool isHorizontal) => _primary.GetRepresentation(isHorizontal);
        public ConsoleColor GetColor() => _primary.GetColor();
        public bool IsOpen(Player player, Room currentRoom)
        {
            return _primary.IsOpen(player, currentRoom) && _secondary.IsOpen(player, currentRoom);
        }
        public void NotifyStateChange()
        {
            _primary.NotifyStateChange();
            _secondary.NotifyStateChange();
        }
    }

}
