using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.BusinessLogic.Struct
{
    public struct Coordinates
    {
        public int X { get; }
        public int Y { get; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        // Add useful methods and operators
        public static Coordinates operator +(Coordinates a, Coordinates b)
        {
            return new Coordinates(a.X + b.X, a.Y + b.Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
