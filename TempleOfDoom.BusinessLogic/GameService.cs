using System.Linq;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Strategies;
using TempleOfDoom.BusinessLogic.Services;
using CODE_TempleOfDoom_DownloadableContent;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Models.Items;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic
{
    public class GameService
    {
        private Dictionary<int, Room> _roomsById;
        private IGameStateManager _gameStateManager;
        private IPlayerMovementController _playerMovementController;

        public Room CurrentRoom { get; private set; }
        public Player Player { get; private set; }

        public bool IsWin => _gameStateManager.IsWin;
        public bool IsLose => _gameStateManager.IsLose;
        public bool IsGameOver => _gameStateManager.IsGameOver;

        public GameService(
            IGameStateManager gameStateManager,
            Room currentRoom,
            Player player,
            Dictionary<int, Room> roomsById,
            Dictionary<int, Dictionary<Direction, int>> roomConnections)
        {
            _gameStateManager = gameStateManager;
            _roomsById = roomsById;

            int totalStones = CountAllSankaraStones(_roomsById.Values);
            _gameStateManager.SetTotalStones(totalStones);

            IDoorService doorService = new DoorService();
            IRoomTransitionService roomTransitionService =
                new RoomTransitionService(roomConnections, doorService, _roomsById);

            IMovementStrategy movementStrategy = new DefaultMovementStrategy();
            _playerMovementController = new PlayerMovementController(
                movementStrategy,
                roomTransitionService,
                doorService,
                _gameStateManager
            );

            CurrentRoom = currentRoom;
            Player = player;
        }

        public void HandlePlayerMovement(Direction direction)
        {
            if (IsGameOver) return;

            // 1. Move the player
            if (_playerMovementController.TryMovePlayer(Player, CurrentRoom, direction, out var newRoom))
            {
                if (newRoom != CurrentRoom)
                {
                    SetCurrentRoom(newRoom);
                }
            }

            // 2. Now move enemies in the current room
            MoveEnemiesInRoom(CurrentRoom);

            // 3. (Optional) Check if the player has collided with an enemy, etc.
            //    For example, if your design says "if you step on an enemy, you take damage"
            //    you'd do it here. Or you might handle that in the Move() logic, up to you.
        }

        private void MoveEnemiesInRoom(Room room)
        {
            // Snapshot the list in case enemies die mid-loop
            var enemiesSnapshot = room.Enemies.ToList();

            foreach (var enemy in enemiesSnapshot)
            {
                if (room.Enemies.Contains(enemy))
                {
                    enemy.Move();  // The big moment:
                                   // This calls IField.Move() → moves to neighbor.
                                   // Because we set CurrentField, everything should line up.
                }
            }
        }

        public void SetCurrentRoom(Room room)
        {
            CurrentRoom = room;
        }

        private int CountAllSankaraStones(IEnumerable<Room> rooms)
        {
            int count = 0;
            foreach (var room in rooms)
            {
                for (int y = 0; y < room.Height; y++)
                {
                    for (int x = 0; x < room.Width; x++)
                    {
                        if (room.Layout[y, x] is FloorTile floorTile
                            && floorTile.Item is SankaraStoneDecorator)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        public IEnumerable<(int X, int Y)> GetEnemyPositions(Room room)
        {
            // We can see the real library's Enemy here because BusinessLogic references it.
            return room.Enemies.Select(e => (e.CurrentXLocation, e.CurrentYLocation));
        }

        public void HandlePlayerShoot()
        {
            if (IsGameOver) return; // if the game is already over, do nothing

            // 1. Calculate the 4 adjacent positions
            var pPos = Player.Position;
            var adjacentPositions = new List<Coordinates>
            {
                new Coordinates(pPos.X,     pPos.Y - 1), // North
                new Coordinates(pPos.X + 1, pPos.Y    ), // East
                new Coordinates(pPos.X,     pPos.Y + 1), // South
                new Coordinates(pPos.X - 1, pPos.Y    )  // West
            };

            // 2. Loop the enemies in the current room
            //    If an enemy is in an adjacent position, do damage.
            var enemiesSnapshot = CurrentRoom.Enemies.ToList();
            foreach (var enemy in enemiesSnapshot)
            {
                // Check if that enemy is in one of the 4 adjacent squares:
                bool isAdjacent = adjacentPositions.Any(pos =>
                    pos.X == enemy.CurrentXLocation &&
                    pos.Y == enemy.CurrentYLocation);

                if (isAdjacent)
                {
                    // 3. Damage the enemy by 1
                    enemy.DoDamage(1);

                    // If the enemy dies, the OnDeath event automatically
                    // removes it from the room (as we've set up earlier).
                }
            }
        }
    }
}
