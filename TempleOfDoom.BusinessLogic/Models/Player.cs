using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Models
{
    public class Player
    {
        public Coordinates Position { get; set; }
        public int Lives { get; set; }

        public Player(int x, int y, int lives = 3)
        {
            Position = new Coordinates(x, y);
            Lives = lives;
        }

        public void Move(Direction direction)
        {
            Coordinates movement = direction switch
            {
                Direction.North => new Coordinates(0, -1),
                Direction.South => new Coordinates(0, 1),
                Direction.West => new Coordinates(-1, 0),
                Direction.East => new Coordinates(1, 0),
                _ => new Coordinates(0, 0)
            };

            Position += movement;
        }
    }
}

