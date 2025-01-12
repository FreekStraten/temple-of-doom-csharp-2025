using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Struct;
using TempleOfDoom.BusinessLogic.Helpers;

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

            // If game over, do nothing
            if (_gameStateManager.IsGameOver)
                return false;

            // 1) Determine the next in-room position (just a single tile step).
            Coordinates nextPos = _movementStrategy.GetNextPosition(player, currentRoom, direction);

            // 2) If that position is within the bounds of the room...
            if (IsInsideRoom(nextPos, currentRoom))
            {
                // 2a) Check if that tile is walkable
                if (IsPositionWalkable(currentRoom, player, nextPos))
                {
                    // 2b) Move the player onto that tile
                    player.UpdatePosition(nextPos);

                    // 2c) Check for any item on that tile
                    HandleItemOnNewPosition(player, currentRoom, nextPos);
                    if (player.Lives <= 0)
                    {
                        // Player died from an item (e.g. boobytrap)
                        _gameStateManager.MarkLose();
                        return true;
                    }

                    // 2d) Check if that tile is a LadderTile => might instantly move to a new room
                    if (currentRoom.GetTileAt(nextPos) is LadderTile ladderTile)
                    {
                        // Handle the ladder transition
                        newCurrentRoom = HandleLadderTransition(player, ladderTile);
                        return true;
                    }

                    // 2e) Finally, if the tile is ice, keep sliding
                    SlidePlayerIfOnIce(player, ref newCurrentRoom, direction);

                    return true;
                }
                else
                {
                    // Tile isn't walkable => cannot move there
                    return false;
                }
            }
            else
            {
                // 3) We’re out-of-bounds, so see if there’s a door transition to another room
                if (_doorService.CanPassThroughDoor(player, currentRoom, direction))
                {
                    // Actually transition rooms
                    if (_roomTransitionService.TryTransition(currentRoom, player, direction, out Room nextRoom))
                    {
                        // After transitioning, the player is in nextRoom at the door’s entry position
                        // Check if there’s an item in that new position
                        HandleItemOnNewPosition(player, nextRoom, player.Position);
                        if (player.Lives <= 0)
                        {
                            _gameStateManager.MarkLose();
                            newCurrentRoom = nextRoom;
                            return true;
                        }

                        // New room is set
                        newCurrentRoom = nextRoom;
                        return true;
                    }
                }

                // No valid door or transition => can’t move
                return false;
            }
        }

        /// <summary>
        /// Check if there's an item on the floor at `pos` and call OnPlayerEnter.
        /// Remove the item if the item so indicates.
        /// </summary>
        private void HandleItemOnNewPosition(Player player, Room room, Coordinates pos)
        {
            var tile = room.GetTileAt(pos);
            if (tile is FloorTile floorTile && floorTile.Item != null)
            {
                bool removeItem = floorTile.Item.OnPlayerEnter(player, room);
                if (removeItem)
                {
                    floorTile.Item = null;
                }
            }
        }

        /// <summary>
        /// Move the player to the LadderTile's connected room & position.
        /// Returns the new Room the player ends up in.
        /// </summary>
        private Room HandleLadderTransition(Player player, LadderTile ladderTile)
        {
            // Find the target room via the roomTransitionService (or a direct dictionary).
            Room targetRoom = _roomTransitionService.FindRoomById(ladderTile.ConnectedRoomId);

            // Teleport player to the target coordinates in the new room
            player.UpdatePosition(ladderTile.TargetCoordinates);

            return targetRoom;
        }


        /// <summary>
        /// Continues moving the player in the same `direction` if the tile is ice.
        /// Updates the currentRoom reference if a ladder or door transition occurs mid-slide.
        /// </summary>
        private void SlidePlayerIfOnIce(Player player, ref Room currentRoom, Direction direction)
        {
            // Safety counter to avoid infinite loops
            for (int i = 0; i < 50; i++)
            {
                // Check the tile the player is currently standing on
                var tile = currentRoom.GetTileAt(player.Position);
                if (tile is IceTile)
                {
                    // Calculate the next position in the same direction
                    Coordinates movement = DirectionHelper.ToCoordinates(direction);
                    Coordinates nextPos = player.Position + movement;

                    // If next position is out of bounds => done sliding
                    if (!IsInsideRoom(nextPos, currentRoom))
                        break;

                    // If next tile is not walkable => done sliding
                    if (!IsPositionWalkable(currentRoom, player, nextPos))
                        break;

                    // Move onto that tile
                    player.UpdatePosition(nextPos);

                    // Handle any item on the tile
                    HandleItemOnNewPosition(player, currentRoom, nextPos);
                    if (player.Lives <= 0)
                    {
                        // Player died mid-slide
                        _gameStateManager.MarkLose();
                        return;
                    }

                    // If it’s a LadderTile, instantly climb => end sliding
                    if (currentRoom.GetTileAt(nextPos) is LadderTile ladderTile)
                    {
                        currentRoom = HandleLadderTransition(player, ladderTile);
                        // Once you switch rooms, there's no more sliding 
                        break;
                    }
                }
                else
                {
                    // Not on ice => stop sliding
                    break;
                }
            }
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

