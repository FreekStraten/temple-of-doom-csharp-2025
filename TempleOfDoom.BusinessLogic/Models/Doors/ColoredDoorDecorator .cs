using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Models.Item.Item;
using TempleOfDoom.BusinessLogic.Models.Items; // <-- for KeyDecorator if that's where you placed it

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
            // Example: '=' for horizontal, '|' for vertical
            return isHorizontal ? '=' : '|';
        }

        public override ConsoleColor GetColor()
        {
            // Override the color based on _color
            return _color switch
            {
                "red" => ConsoleColor.Red,
                "green" => ConsoleColor.Green,
                _ => ConsoleColor.DarkGray
            };
        }

        public override bool IsOpen(Player player, Room currentRoom)
        {
            // Check if the player’s inventory contains a KeyDecorator of the matching color
            bool hasMatchingKey = player.Inventory.Any(i => i is KeyDecorator key
                                                         && key.Color.ToLower() == _color);

            // Only open if the player has that key AND the wrapped door is open
            return hasMatchingKey && base.IsOpen(player, currentRoom);
        }
    }
}
