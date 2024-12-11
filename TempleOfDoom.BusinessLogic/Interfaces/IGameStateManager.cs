using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.BusinessLogic.Interfaces
{
    public interface IGameStateManager
    {
        bool IsWin { get; }
        bool IsLose { get; }
        bool IsGameOver { get; }

        void MarkWin();
        void MarkLose();
        // Possibly notify about life changes, stone count changes, etc.
    }

}
