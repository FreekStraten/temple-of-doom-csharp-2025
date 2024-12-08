using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Item.Item;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class ColoredDoor : IDoor
    {
        private readonly string _color;
        public ColoredDoor(string color)
        {
            _color = color.ToLower();
        }

        public char GetRepresentation(bool isHorizontal) => isHorizontal ? '=' : '|';

        public ConsoleColor GetColor()
        {
            return _color switch
            {
                "red" => ConsoleColor.Red,
                "green" => ConsoleColor.Green,
                _ => ConsoleColor.DarkGray
            };
        }

        public bool IsOpen(Player player, Room currentRoom)
        {
            // Open if player has a key of this color
            return player.Inventory.Any(i => i is KeyItem key && key.Color.ToLower() == _color);
        }

        public void NotifyStateChange() { /* Not needed for colored door */ }
    }
}
