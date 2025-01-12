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

            // 1) Determine the next position (inside the same room) the player wants to move to
            Coordinates nextPos = _movementStrategy.GetNextPosition(player, currentRoom, direction);

            // 2) If that position is still within the current room’s bounds...
            if (IsInsideRoom(nextPos, currentRoom))
            {
                // 2a) Check if the tile is walkable (floor or open door)
                if (IsPositionWalkable(currentRoom, player, nextPos))
                {
                    // 2b) Move the player
                    player.UpdatePosition(nextPos);

                    // 2c) Check if there's an item on the floor
                    if (currentRoom.GetTileAt(nextPos) is FloorTile floorTile && floorTile.Item != null)
                    {
                        bool removeItem = floorTile.Item.OnPlayerEnter(player, currentRoom);

                        // If the item damaged the player to 0 or below, game over
                        if (player.Lives <= 0)
                        {
                            _gameStateManager.MarkLose();
                            return true;
                        }

                        // If OnPlayerEnter(...) said "true," remove item from the tile
                        if (removeItem)
                        {
                            floorTile.Item = null;
                        }
                    }

                    // ---------------------------------------------------------
                    // 3) NEW CODE: If the player is now standing on a LadderTile,
                    //    do an automatic ladder transition to the connected room.
                    // ---------------------------------------------------------
                    if (currentRoom.GetTileAt(nextPos) is LadderTile ladderTile)
                    {
                        // Get the target room
                        Room ladderTargetRoom = _roomTransitionService.FindRoomById(ladderTile.ConnectedRoomId);

                        // Move the player to that ladder’s targetCoordinates in the new room
                        player.UpdatePosition(ladderTile.TargetCoordinates);

                        // Return that new room as our "currentRoom"
                        newCurrentRoom = ladderTargetRoom;
                        return true;
                    }

                    // Otherwise, we just moved within the same room
                    return true;
                }
                return false;
            }
            else
            {
                // 4) We are out-of-bounds => try passing through a door to another room
                if (_doorService.CanPassThroughDoor(player, currentRoom, direction))
                {
                    if (_roomTransitionService.TryTransition(currentRoom, player, direction, out var nextRoom))
                    {
                        // After transitioning, the player is now in nextRoom at the door’s entry position
                        // Possibly also pick up an item if there's one on the floor in the new position
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

