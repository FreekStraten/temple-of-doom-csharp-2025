using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Tile
{
    public abstract class Tile : ITile
    {
        public abstract string Representation { get; }
        public abstract bool IsWalkable { get; }
    }

}
