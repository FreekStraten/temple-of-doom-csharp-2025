using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Manager;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Services;
using TempleOfDoom.BusinessLogic.Strategies;
using TempleOfDoom.BusinessLogic;

public class GameService
{
    // Public for demonstration; better to inject or pass through constructor
    public static Dictionary<int, Room> RoomsById { get; set; }

    public static GameService Instance { get; private set; }

    private IGameStateManager _gameStateManager;
    private IPlayerMovementController _playerMovementController;
    private IItemCollector _itemCollector;

    public Room CurrentRoom { get; private set; }
    public Player Player { get; private set; }

    public bool IsWin => _gameStateManager.IsWin;
    public bool IsLose => _gameStateManager.IsLose;
    public bool IsGameOver => _gameStateManager.IsGameOver;

    public GameService(
        Room currentRoom,
        Player player,
        Dictionary<int, Room> roomsById,
        Dictionary<int, Dictionary<Direction, int>> roomConnections)
    {
        Instance = this;
        RoomsById = roomsById;

        _gameStateManager = new GameStateManager();

        // Setup services
        IDoorService doorService = new DoorService();
        IItemCollector itemCollector = new ItemCollector();
        itemCollector.InitializeTotalStones(RoomsById.Values);
        itemCollector.SetGameStateManager(_gameStateManager);

        IRoomTransitionService roomTransitionService = new RoomTransitionService(roomConnections, doorService);
        IMovementStrategy movementStrategy = new DefaultMovementStrategy();
        IPlayerMovementController movementController = new PlayerMovementController(movementStrategy, roomTransitionService, doorService, itemCollector, _gameStateManager);

        _itemCollector = itemCollector;
        _playerMovementController = movementController;

        CurrentRoom = currentRoom;
        Player = player;
    }

    public void HandlePlayerMovement(Direction direction)
    {
        if (IsGameOver) return;

        _playerMovementController.TryMovePlayer(Player, CurrentRoom, direction);
    }

    public void SetCurrentRoom(Room room)
    {
        CurrentRoom = room;
    }
}