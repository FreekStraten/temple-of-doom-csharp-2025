using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.BusinessLogic.Models.Tile
{
    public class WallTile : Tile
    {
        public override string Representation => "#";
        public override bool IsWalkable => false;
    }

}
