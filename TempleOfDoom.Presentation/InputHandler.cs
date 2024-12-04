using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Enum;

namespace TempleOfDoom.Presentation
{
    public static class InputHandler
    {
        public static Direction? GetDirectionFromInput(ConsoleKey key)
        {
            return key switch
            {
                ConsoleKey.UpArrow or ConsoleKey.W => Direction.North,
                ConsoleKey.DownArrow or ConsoleKey.S => Direction.South,
                ConsoleKey.LeftArrow or ConsoleKey.A => Direction.West,
                ConsoleKey.RightArrow or ConsoleKey.D => Direction.East,
                _ => null
            };
        }
    }

}
