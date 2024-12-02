using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.DataAccess
{
    public class LevelDto
    {
        public List<RoomDto> Rooms { get; set; }
        public List<ConnectionDto> Connections { get; set; }
        public PlayerDto Player { get; set; }
    }
}
