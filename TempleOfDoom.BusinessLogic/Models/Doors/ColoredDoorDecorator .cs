using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Item.Item;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class ColoredDoorDecorator : DoorDecorator
    {
        private readonly string _color;

        public ColoredDoorDecorator(IDoor wrappedDoor, string color)
            : base(wrappedDoor)
        {
            _color = color.ToLower();
        }

        public override char GetRepresentation(bool isHorizontal)
        {
            // If you want a certain char for a colored door, override it here
            // or fallback to the base if you only want to color it.
            return isHorizontal ? '=' : '|';
        }

        public override ConsoleColor GetColor()
        {
            // Instead of returning the wrapped door color, we do the color override
            return _color switch
            {
                "red" => ConsoleColor.Red,
                "green" => ConsoleColor.Green,
                _ => ConsoleColor.DarkGray
            };
        }

        public override bool IsOpen(Player player, Room currentRoom)
        {
            // If the player has the matching key, we can allow open
            bool hasMatchingKey = player.Inventory.Any(i => i is KeyItem key && key.Color.ToLower() == _color);
            return hasMatchingKey && base.IsOpen(player, currentRoom);
        }
    }

}
