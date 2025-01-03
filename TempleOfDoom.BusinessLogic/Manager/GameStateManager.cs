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

        private int _totalStones;
        private int _collectedStones;

        public void MarkWin() => IsWin = true;
        public void MarkLose() => IsLose = true;

        // NEW: Called by GameService (or wherever you prefer) to initialize the total stones
        public void SetTotalStones(int total)
        {
            _totalStones = total;
            _collectedStones = 0;
        }

        // NEW: Called whenever the player picks up a Sankara Stone
        public void OnSankaraStoneCollected()
        {
            _collectedStones++;
            if (_collectedStones >= _totalStones)
            {
                MarkWin();
            }
        }
    }
}

