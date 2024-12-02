using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.DataAccess
{
    public class PlayerDto
    {
        public int StartRoomId { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int Lives { get; set; }
    }
}
