using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.BusinessLogic.Models.Tile
{
    public class LadderTile : Tile
    {
        // We'll just display an 'H' to represent a ladder
        public override string Representation => "H";

        // Let’s assume you can walk onto it
        public override bool IsWalkable => true;
    }
}
