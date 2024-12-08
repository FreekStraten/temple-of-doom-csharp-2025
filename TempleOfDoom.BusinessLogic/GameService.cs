using TempleOfDoom.BusinessLogic;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Models.Items;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Strategies;
using TempleOfDoom.BusinessLogic.Struct;

public class GameService
{
    private readonly Dictionary<int, Room> _roomsById;
    private readonly Dictionary<int, Dictionary<Direction, int>> _roomConnections;
    private readonly IMovementStrategy _movementStrategy;

    private int _totalStones;
    private int _collectedStones;

    public Room CurrentRoom { get; private set; }
    public Player Player { get; private set; }

    public bool IsWin { get; private set; }
    public bool IsLose { get; private set; }

    public bool IsGameOver => IsWin || IsLose;

    public GameService(Room currentRoom, Player player, Dictionary<int, Room> roomsById,
                      Dictionary<int, Dictionary<Direction, int>> roomConnections,
                      IMovementStrategy movementStrategy = null)
    {
        CurrentRoom = currentRoom;
        Player = player;
        _roomsById = roomsById;
        _roomConnections = roomConnections;
        _movementStrategy = movementStrategy ?? new DefaultMovementStrategy();

        CountTotalStones();
    }

    private void CountTotalStones()
    {
        _totalStones = _roomsById.Values
            .SelectMany(room => room.Layout.Cast<ITile>())
            .OfType<ItemTileDecorator>()
            .Count(itemTile => itemTile.Item is SankaraStone);
    }

    public void HandlePlayerMovement(Direction direction)
    {
        Coordinates nextPos = _movementStrategy.GetNextPosition(Player, CurrentRoom, direction);

        if (IsPositionWalkable(nextPos))
        {
            Player.UpdatePosition(nextPos);
            ProcessTile(Player.Position);
        }
        else if (IsMoveOutsideRoom(direction) && CurrentTileIsDoor())
        {
            if (AttemptRoomTransition(direction))
                return;
        }
    }

    private bool IsPositionWalkable(Coordinates pos)
    {
        if (pos.X < 0 || pos.X >= CurrentRoom.Width || pos.Y < 0 || pos.Y >= CurrentRoom.Height)
            return false;

        return CurrentRoom.GetTileAt(pos).IsWalkable;
    }

    private void ProcessTile(Coordinates pos)
    {
        var tile = CurrentRoom.GetTileAt(pos);
        if (tile is ItemTileDecorator itemTile)
        {
            bool shouldRemove = itemTile.Item.OnPlayerEnter(Player);

            if (Player.Lives <= 0)
            {
                IsLose = true;
                return;
            }

            if (shouldRemove && itemTile.Item is SankaraStone)
            {
                _collectedStones++;
                if (_collectedStones == _totalStones)
                {
                    IsWin = true;
                    return;
                }
            }

            if (shouldRemove)
            {
                CurrentRoom.RemoveItemAt(pos);
            }
        }
    }

    private bool IsMoveOutsideRoom(Direction direction)
    {
        Coordinates nextPos = Player.GetNewPosition(direction);
        return nextPos.X < 0 || nextPos.X >= CurrentRoom.Width || nextPos.Y < 0 || nextPos.Y >= CurrentRoom.Height;
    }

    private bool CurrentTileIsDoor()
    {
        return CurrentRoom.GetTileAt(Player.Position) is DoorTile;
    }

    private bool AttemptRoomTransition(Direction direction)
    {
        if (_roomConnections.TryGetValue(CurrentRoom.Id, out var connectionsForRoom) &&
            connectionsForRoom.TryGetValue(direction, out int nextRoomId))
        {
            Room nextRoom = _roomsById[nextRoomId];
            Player.UpdatePosition(GetEntryPositionInNextRoom(nextRoom, OppositeDirection(direction)));
            CurrentRoom = nextRoom;
            return true;
        }
        return false;
    }

    private Direction OppositeDirection(Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.South,
            Direction.South => Direction.North,
            Direction.East => Direction.West,
            Direction.West => Direction.East,
            _ => direction
        };
    }

    private Coordinates GetEntryPositionInNextRoom(Room nextRoom, Direction comingFromDirection)
    {
        return comingFromDirection switch
        {
            Direction.North => new Coordinates(nextRoom.Width / 2, 0),
            Direction.South => new Coordinates(nextRoom.Width / 2, nextRoom.Height - 1),
            Direction.West => new Coordinates(0, nextRoom.Height / 2),
            Direction.East => new Coordinates(nextRoom.Width - 1, nextRoom.Height / 2),
            _ => new Coordinates(1, 1)
        };
    }
}
