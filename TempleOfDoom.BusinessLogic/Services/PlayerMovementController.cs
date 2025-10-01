using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Models.Tile;
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

            if (_gameStateManager.IsGameOver)
                return false;

            // 1) Bepaal de eerstvolgende positie (enkel 1 stap)
            Coordinates nextPos = _movementStrategy.GetNextPosition(player, currentRoom, direction);

            // 2) Binnen de kamer?
            if (IsInsideRoom(nextPos, currentRoom))
            {
                // 2a) Walkable?
                if (IsPositionWalkable(currentRoom, player, nextPos))
                {
                    // 2b) Move naar die tegel
                    player.UpdatePosition(nextPos);

                    // 2c) Item oppakken (als aanwezig)
                    HandleItemOnNewPosition(player, currentRoom, nextPos);
                    if (player.Lives <= 0)
                    {
                        _gameStateManager.MarkLose();
                        return true;
                    }

                    // 2d) Ladder direct na deze stap?
                    var enteredTileImmediate = currentRoom.GetTileAt(nextPos);
                    if (enteredTileImmediate is LadderTile ladderNow)
                    {
                        newCurrentRoom = HandleLadderTransition(player, ladderNow);
                        return true;
                    }

                    // 2e) Auto-moves (bijv. ICE): laat de tile het bepalen
                    if (enteredTileImmediate is IAutoMoveTile autoTile)
                    {
                        foreach (var extraPos in autoTile.GetAutoMoves(currentRoom, player.Position, direction, player))
                        {
                            // Veiligheidschecks (zou al in GetAutoMoves zitten, maar we borgen het hier nogmaals)
                            if (!IsInsideRoom(extraPos, currentRoom)) break;
                            if (!IsPositionWalkable(currentRoom, player, extraPos)) break;

                            player.UpdatePosition(extraPos);

                            // Items tijdens het glijden
                            HandleItemOnNewPosition(player, currentRoom, extraPos);
                            if (player.Lives <= 0)
                            {
                                _gameStateManager.MarkLose();
                                return true;
                            }

                            // Ladder tijdens glijden? Direct transition en stoppen
                            var tileNow = currentRoom.GetTileAt(extraPos);
                            if (tileNow is LadderTile ladderDuringSlide)
                            {
                                newCurrentRoom = HandleLadderTransition(player, ladderDuringSlide);
                                return true;
                            }
                        }
                    }

                    return true;
                }

                // Tile niet walkable
                return false;
            }
            else
            {
                // 3) Buiten de kamergrenzen: probeer deur/room-transition
                if (_doorService.CanPassThroughDoor(player, currentRoom, direction))
                {
                    if (_roomTransitionService.TryTransition(currentRoom, player, direction, out Room nextRoom))
                    {
                        // In nieuwe room aangekomen
                        HandleItemOnNewPosition(player, nextRoom, player.Position);
                        if (player.Lives <= 0)
                        {
                            _gameStateManager.MarkLose();
                            newCurrentRoom = nextRoom;
                            return true;
                        }

                        newCurrentRoom = nextRoom;
                        return true;
                    }
                }

                // Geen overgang mogelijk
                return false;
            }
        }

        /// <summary>
        /// Check op item en voer OnPlayerEnter uit; verwijder item indien nodig.
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
        /// Ladderovergang uitvoeren en nieuwe kamer teruggeven.
        /// </summary>
        private Room HandleLadderTransition(Player player, LadderTile ladderTile)
        {
            Room targetRoom = _roomTransitionService.FindRoomById(ladderTile.ConnectedRoomId);
            player.UpdatePosition(ladderTile.TargetCoordinates);
            return targetRoom;
        }

        private bool IsInsideRoom(Coordinates pos, Room room)
            => pos.X >= 0 && pos.X < room.Width && pos.Y >= 0 && pos.Y < room.Height;

        private bool IsPositionWalkable(Room room, Player player, Coordinates pos)
        {
            var tile = room.GetTileAt(pos);
            if (tile is DoorTile doorTile)
            {
                // De deurtegel zelf is niet walkable, maar als hij open is mag je er "door" (transition gebeurt elders).
                return doorTile.Door.IsOpen(player, room);
            }
            return tile.IsWalkable;
        }

        // Enemy-glijden laten we ongewijzigd (werkt al prima),
        // maar je kunt dit later ook via IAutoMoveTile laten lopen.
        private void SlideEnemyIfOnIce(CODE_TempleOfDoom_DownloadableContent.Enemy enemy, Room room)
        {
            for (int i = 0; i < 50; i++)
            {
                var ex = enemy.CurrentXLocation;
                var ey = enemy.CurrentYLocation;
                if (ex < 0 || ex >= room.Width || ey < 0 || ey >= room.Height) break;

                var tile = room.GetTileAt(new Coordinates(ex, ey));
                if (tile is IceTile)
                {
                    enemy.Move(); // gebruikt z’n laatst bekende richting
                }
                else
                {
                    break;
                }
            }
        }
    }
}
