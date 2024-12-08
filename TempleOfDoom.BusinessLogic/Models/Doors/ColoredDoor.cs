using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class ColoredDoor : IDoor
    {
        private readonly string _color;
        public ColoredDoor(string color)
        {
            _color = color.ToLower();
        }

        public char GetRepresentation(bool isHorizontal)
            => isHorizontal ? '=' : '|';

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
            // Logic not yet implemented
            // Placeholder: always closed for now
            return false;
        }

        public void NotifyStateChange() { /* Not used yet */ }
    }
}
