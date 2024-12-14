using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Items;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Models.Items.TempleOfDoom.BusinessLogic.Models.Items;

namespace TempleOfDoom.BusinessLogic
{
    public class ItemCollector : IItemCollector
    {
        private int _totalStones;
        private int _collectedStones;
        private IGameStateManager _gameStateManager;

        public void InitializeTotalStones(IEnumerable<Room> rooms)
        {
            _totalStones = rooms.SelectMany(r => r.Layout.Cast<ITile>())
                .OfType<ItemTileDecorator>()
                .Count(t => t.Item is SankaraStone);
        }

        public void SetGameStateManager(IGameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public bool HandleItemInteraction(Player player, Room room)
        {
            var tile = room.GetTileAt(player.Position);
            if (tile is ItemTileDecorator itemTile)
            {
                bool shouldRemove = itemTile.Item.OnPlayerEnter(player, room);
                if (player.Lives <= 0)
                {
                    _gameStateManager?.MarkLose();
                    return true;
                }
                if (shouldRemove)
                {
                    if (itemTile.Item is SankaraStone)
                    {
                        _collectedStones++;
                        if (_collectedStones == _totalStones)
                        {
                            _gameStateManager?.MarkWin();
                        }
                    }
                    room.RemoveItemAt(player.Position);
                }
            }
            return false;
        }
    }
}
