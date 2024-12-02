using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.BusinessLogic.Models
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; } = 3;

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.North: Y--; break;
                case Direction.East: X++; break;
                case Direction.South: Y++; break;
                case Direction.West: X--; break;
            }
        }
    }

    public enum Direction
    {
        North,
        East,
        South,
        West
    }
}

