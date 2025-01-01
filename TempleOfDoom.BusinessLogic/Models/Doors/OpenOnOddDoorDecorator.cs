using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class OpenOnOddDoorDecorator : DoorDecorator
    {
        public OpenOnOddDoorDecorator(IDoor wrappedDoor)
            : base(wrappedDoor)
        { }

        public override char GetRepresentation(bool isHorizontal)
        {
            return base.GetRepresentation(isHorizontal);
        }

        public override ConsoleColor GetColor()
        {
            return base.GetColor();
        }

        public override bool IsOpen(Player player, Room currentRoom)
        {
            // Check base chain *and* the odd-lives condition
            return base.IsOpen(player, currentRoom) && (player.Lives % 2 != 0);
        }
    }
}
