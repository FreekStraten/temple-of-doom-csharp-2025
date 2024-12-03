using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Models
{
    public class Item
    {
        public Coordinates Position { get; set; }
        public string Type { get; set; }

        public Item(int x, int y, string type)
        {
            Position = new Coordinates(x, y);
            Type = type;
        }
    }
}
