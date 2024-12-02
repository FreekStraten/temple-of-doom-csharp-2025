using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.DataAccess
{
    public class ConnectionDto
    {
        public int? NORTH { get; set; }
        public int? SOUTH { get; set; }
        public int? EAST { get; set; }
        public int? WEST { get; set; }
        public List<DoorDto> Doors { get; set; }
    }
}
