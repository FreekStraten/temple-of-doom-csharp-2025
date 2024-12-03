using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.BusinessLogic.Interfaces
{
    public interface ITile
    {
        string Representation { get; }
        bool IsWalkable { get; }
    }
}
