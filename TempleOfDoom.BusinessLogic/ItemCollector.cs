using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Items;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Models.Tile;

namespace TempleOfDoom.BusinessLogic
{
    public class ItemCollector : IItemCollector
    {
        private int _totalStones;
        private int _collectedStones;
        private IGameStateManager _gameStateManager;

        public void InitializeTotalStones(IEnumerable<Room> rooms)
        {
            // Look through all rooms' layout and count how many floor tiles
            // have an item that is a SankaraStone (decorator).
            _totalStones = rooms
                .SelectMany(r => r.Layout.Cast<ITile>())
                .OfType<FloorTile>() // only floor tiles hold items
                .Count(floorTile => floorTile.Item is SankaraStoneDecorator);
        }

        public void SetGameStateManager(IGameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public bool HandleItemInteraction(Player player, Room room)
        {
            // Get the tile where the player is standing
            ITile tile = room.GetTileAt(player.Position);

            // If it's a FloorTile that has an IItem
            if (tile is FloorTile floorTile && floorTile.Item != null)
            {
                // Let the item handle the interaction
                bool shouldRemove = floorTile.Item.OnPlayerEnter(player, room);

                // If the player’s lives dropped to 0 or below, game over
                if (player.Lives <= 0)
                {
                    _gameStateManager?.MarkLose();
                    return true;
                }

                // If the item indicates it should be removed from the tile
                if (shouldRemove)
                {
                    // Check if it’s a SankaraStone to increment our counter
                    if (floorTile.Item is SankaraStoneDecorator)
                    {
                        _collectedStones++;
                        if (_collectedStones == _totalStones)
                        {
                            _gameStateManager?.MarkWin();
                        }
                    }
                    // Remove the item by setting it to null
                    floorTile.Item = null;
                }
            }

            return false;
        }
    }
}
