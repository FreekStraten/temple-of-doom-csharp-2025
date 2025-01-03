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
        private readonly IGameStateManager _gameStateManager;

        public PlayerMovementController(
            IMovementStrategy movementStrategy,
            IRoomTransitionService roomTransitionService,
            IDoorService doorService,
            IGameStateManager gameStateManager)
        {
            _movementStrategy = movementStrategy;
            _roomTransitionService = roomTransitionService;
            _doorService = doorService;
            _gameStateManager = gameStateManager;
        }

        public bool TryMovePlayer(Player player, Room currentRoom, Direction direction, out Room newCurrentRoom)
        {
            newCurrentRoom = currentRoom;
            if (_gameStateManager.IsGameOver) return false;

            Coordinates nextPos = _movementStrategy.GetNextPosition(player, currentRoom, direction);
            if (IsInsideRoom(nextPos, currentRoom))
            {
                // Movement inside this room
                if (IsPositionWalkable(currentRoom, player, nextPos))
                {
                    player.UpdatePosition(nextPos);

                    // Check if there's an item to interact with
                    if (currentRoom.GetTileAt(nextPos) is FloorTile floorTile && floorTile.Item != null)
                    {
                        bool removeItem = floorTile.Item.OnPlayerEnter(player, currentRoom);

                        // If player's lives are now <= 0, game over
                        if (player.Lives <= 0)
                        {
                            _gameStateManager.MarkLose();
                            return true;
                        }

                        // If OnPlayerEnter(...) says "true," remove the item
                        if (removeItem)
                        {
                            floorTile.Item = null;
                        }
                    }
                    return true;
                }
                return false;
            }
            else
            {
                // Attempt to pass through a door to another room
                if (_doorService.CanPassThroughDoor(player, currentRoom, direction))
                {
                    if (_roomTransitionService.TryTransition(currentRoom, player, direction, out var nextRoom))
                    {
                        // After transitioning, the player is now in nextRoom at the door’s entry position
                        // Optionally do the same item-check in the new room's position
                        if (nextRoom.GetTileAt(player.Position) is FloorTile floorTile && floorTile.Item != null)
                        {
                            bool removeItem = floorTile.Item.OnPlayerEnter(player, nextRoom);

                            if (player.Lives <= 0)
                            {
                                _gameStateManager.MarkLose();
                                return true;
                            }
                            if (removeItem)
                            {
                                floorTile.Item = null;
                            }
                        }
                        newCurrentRoom = nextRoom;
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

