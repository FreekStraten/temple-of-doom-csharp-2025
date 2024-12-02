using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.DataAccess
{
    public class ItemDto
    {
        public string Type { get; set; }
        public int? Damage { get; set; } // Nullable for optional properties
        public string Color { get; set; } // Nullable for optional properties
        public int X { get; set; }
        public int Y { get; set; }
    }
}
