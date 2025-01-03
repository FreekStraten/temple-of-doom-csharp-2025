using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Manager;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Services;
using TempleOfDoom.BusinessLogic.Strategies;
using TempleOfDoom.BusinessLogic;
using TempleOfDoom.BusinessLogic.Models.Items;
using TempleOfDoom.BusinessLogic.Models.Tile;

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

        // Count total Sankara Stones in all rooms
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

    public void HandlePlayerMovement(Direction direction)
    {
        if (IsGameOver) return;

        if (_playerMovementController.TryMovePlayer(Player, CurrentRoom, direction, out var newRoom))
        {
            if (newRoom != CurrentRoom)
            {
                SetCurrentRoom(newRoom);
            }
        }
    }

    public void SetCurrentRoom(Room room)
    {
        CurrentRoom = room;
    }
}
