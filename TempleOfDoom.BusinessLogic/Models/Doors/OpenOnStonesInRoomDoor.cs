using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class OpenOnStonesInRoomDoor : IDoor
    {
        private readonly int _requiredStones;
        public OpenOnStonesInRoomDoor(int requiredStones)
        {
            _requiredStones = requiredStones;
        }

        public char GetRepresentation(bool isHorizontal)
            => isHorizontal ? '=' : '|';

        public ConsoleColor GetColor() => ConsoleColor.Blue;

        public bool IsOpen(Player player, Room currentRoom)
        {
            // Placeholder: always closed
            return false;
        }

        public void NotifyStateChange() { }
    }
}
