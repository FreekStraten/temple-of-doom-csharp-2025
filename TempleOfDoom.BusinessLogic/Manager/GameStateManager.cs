using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Manager
{
    public class GameStateManager : IGameStateManager
    {
        public bool IsWin { get; private set; }
        public bool IsLose { get; private set; }
        public bool IsGameOver => IsWin || IsLose;

        public void MarkWin() => IsWin = true;
        public void MarkLose() => IsLose = true;
    }

}
