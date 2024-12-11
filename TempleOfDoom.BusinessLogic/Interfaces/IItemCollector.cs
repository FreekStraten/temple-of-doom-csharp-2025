using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Models;

namespace TempleOfDoom.BusinessLogic.Interfaces
{
    public interface IItemCollector
    {
        void InitializeTotalStones(IEnumerable<Room> rooms);
        bool HandleItemInteraction(Player player, Room room);
        // Possibly a method to check if all stones collected and notify state manager
        void SetGameStateManager(IGameStateManager gameStateManager);
    }
}
