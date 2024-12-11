using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Services
{
    public class PlayerMovementController : IPlayerMovementController
    {
        private readonly IMovementStrategy _movementStrategy;
        private readonly IRoomTransitionService _roomTransitionService;
        private readonly IDoorService _doorService;
        private readonly IItemCollector _itemCollector;
        private readonly IGameStateManager _gameStateManager;

        public PlayerMovementController(
            IMovementStrategy movementStrategy,
            IRoomTransitionService roomTransitionService,
            IDoorService doorService,
            IItemCollector itemCollector,
            IGameStateManager gameStateManager)
        {
            _movementStrategy = movementStrategy;
            _roomTransitionService = roomTransitionService;
            _doorService = doorService;
            _itemCollector = itemCollector;
            _gameStateManager = gameStateManager;
        }

        public bool TryMovePlayer(Player player, Room currentRoom, Direction direction)
        {
            if (_gameStateManager.IsGameOver) return false;

            Coordinates nextPos = _movementStrategy.GetNextPosition(player, currentRoom, direction);
            // Check boundaries
            if (IsInsideRoom(nextPos, currentRoom))
            {
                // Check if walkable or if it's a door
                if (IsPositionWalkable(currentRoom, player, nextPos))
                {
                    player.UpdatePosition(nextPos);
                    _itemCollector.HandleItemInteraction(player, currentRoom); // may set win/lose
                    return true;
                }
                return false;
            }
            else
            {
                // Outside room boundaries, check if door allows passing
                if (_doorService.CanPassThroughDoor(player, currentRoom, direction))
                {
                    // Attempt to transition to next room
                    if (_roomTransitionService.TryTransition(currentRoom, player, direction, out var nextRoom))
                    {
                        // On entering the next room, handle item again (if standing on item)
                        _itemCollector.HandleItemInteraction(player, nextRoom);
                        GameService.Instance.SetCurrentRoom(nextRoom);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsInsideRoom(Coordinates pos, Room room)
        {
            return pos.X >= 0 && pos.X < room.Width && pos.Y >= 0 && pos.Y < room.Height;
        }

        private bool IsPositionWalkable(Room room, Player player, Coordinates pos)
        {
            var tile = room.GetTileAt(pos);
            if (tile is DoorTile doorTile)
            {
                return doorTile.Door.IsOpen(player, room);
            }
            return tile.IsWalkable;
        }
    }

}
