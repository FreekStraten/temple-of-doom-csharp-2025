using System.Collections.Generic;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Interfaces
{
    /// <summary>
    /// Optionele interface voor tiles die na betreden automatisch extra
    /// stappen willen uitvoeren (bijv. glijden op ijs).
    /// </summary>
    public interface IAutoMoveTile
    {
        /// <summary>
        /// Geeft de extra posities (na de eerste stap) waar de speler
        /// automatisch langs beweegt. Stop zodra de enumerable stopt.
        /// </summary>
        IEnumerable<Coordinates> GetAutoMoves(Room room, Coordinates start, Direction direction, Player player);
    }
}
