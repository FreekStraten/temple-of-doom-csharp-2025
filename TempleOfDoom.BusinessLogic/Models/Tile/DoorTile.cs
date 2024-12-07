using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.BusinessLogic.Models.Tile
{
   
    public class DoorTile : Tile
    {
        // For now, a door is represented by a space to indicate openness.
        public override string Representation => " ";
        public override bool IsWalkable => true;
    }
}
