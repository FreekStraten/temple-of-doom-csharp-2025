using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Tile
{
    public class FloorTile : Tile
    {
        public IItem Item { get; set; }  // optional, or pass in constructor

        public override string Representation
        {
            get
            {
                // If there's an item, show item’s representation
                if (Item != null) return Item.Representation;
                return " "; // empty floor
            }
        }

        public override bool IsWalkable => true;
    }
}
