using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.DataAccess
{
    public class DoorDto
    {
        public string Type { get; set; }
        public string Color { get; set; } // Nullable for optional properties
        public int? NoOfStones { get; set; } // Nullable for optional properties
    }

}
