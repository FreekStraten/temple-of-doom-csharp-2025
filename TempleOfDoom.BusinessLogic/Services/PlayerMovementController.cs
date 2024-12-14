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

        public bool TryMovePlayer(Player player, Room currentRoom, Direction direction, out Room newCurrentRoom) // CHANGED
        {
            newCurrentRoom = currentRoom; // default to currentRoom
            if (_gameStateManager.IsGameOver) return false;

            Coordinates nextPos = _movementStrategy.GetNextPosition(player, currentRoom, direction);

            if (IsInsideRoom(nextPos, currentRoom))
            {
                // Inside room movement
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
                // Attempt room transition
                if (_doorService.CanPassThroughDoor(player, currentRoom, direction))
                {
                    if (_roomTransitionService.TryTransition(currentRoom, player, direction, out var nextRoom))
                    {
                        _itemCollector.HandleItemInteraction(player, nextRoom);
                        newCurrentRoom = nextRoom; // CHANGED: Instead of setting via GameService, return it
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
