using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Helpers
{
    public static class DirectionHelper
    {
        public static Coordinates ToCoordinates(Direction direction)
        {
            return direction switch
            {
                Direction.North => new Coordinates(0, -1),
                Direction.South => new Coordinates(0, 1),
                Direction.West => new Coordinates(-1, 0),
                Direction.East => new Coordinates(1, 0),
                _ => new Coordinates(0, 0)
            };
        }
    }
}
