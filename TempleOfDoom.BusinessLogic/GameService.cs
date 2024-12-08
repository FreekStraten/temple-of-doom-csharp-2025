using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Models.Doors;
using TempleOfDoom.BusinessLogic.Models.Items;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Strategies;
using TempleOfDoom.BusinessLogic.Struct;

public class GameService
{
    public static GameService Instance { get; private set; }

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
        Instance = this;
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

        ITile tile = CurrentRoom.GetTileAt(pos);
        if (tile is DoorTile doorTile)
        {
            // Now we have access to Player and CurrentRoom here.
            return doorTile.Door.IsOpen(Player, CurrentRoom);
        }

        return tile.IsWalkable;
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
        var tile = CurrentRoom.GetTileAt(Player.Position);
        return tile is DoorTile;
    }

    private bool AttemptRoomTransition(Direction direction)
    {
        if (_roomConnections.TryGetValue(CurrentRoom.Id, out var connectionsForRoom) &&
            connectionsForRoom.TryGetValue(direction, out int nextRoomId))
        {
            // Identify door position in current room and handle closing gate
            Coordinates doorPos = GetDoorPositionForDirection(CurrentRoom, direction);
            var doorTile = CurrentRoom.GetTileAt(doorPos) as DoorTile;
            // If we passed through a closing gate door, close it now
            if (doorTile != null && ContainsDoorOfType<ClosingGateDoor>(doorTile.Door))
            {
                // After moving through, notify the door to close
                doorTile.Door.NotifyStateChange();
            }

            Room nextRoom = _roomsById[nextRoomId];
            Player.UpdatePosition(GetEntryPositionInNextRoom(nextRoom, OppositeDirection(direction)));
            CurrentRoom = nextRoom;
            return true;
        }
        return false;
    }

    private bool ContainsDoorOfType<T>(IDoor door) where T : IDoor
    {
        if (door is T)
            return true;

        if (door is DoorDecorator dec)
        {
            // Directly check the primary and secondary doors, no reflection needed:
            return ContainsDoorOfType<T>(dec.PrimaryDoor) || ContainsDoorOfType<T>(dec.SecondaryDoor);
        }

        return false;
    }


    private IDoor GetInnerDoor(DoorDecorator decorator, bool primary)
    {
        // We need a way to access the doors inside decorator.
        // Let's assume we add a helper method in DoorDecorator (not shown above for brevity) 
        // or we can store them as fields. 
        // Since we have access to source, we can do:

        // If we can't alter DoorDecorator, we can reflect or store them.
        // Let's assume we add two public properties in DoorDecorator: PrimaryDoor and SecondaryDoor.

        // After updating DoorDecorator:
        // public IDoor PrimaryDoor => _primary;
        // public IDoor SecondaryDoor => _secondary;

        if (decorator == null) return null;
        return primary ? (decorator.GetType().GetProperty("PrimaryDoor").GetValue(decorator) as IDoor)
                       : (decorator.GetType().GetProperty("SecondaryDoor").GetValue(decorator) as IDoor);
    }

    private Coordinates GetDoorPositionForDirection(Room room, Direction direction)
    {
        return direction switch
        {
            Direction.North => new Coordinates(room.Width / 2, 0),
            Direction.South => new Coordinates(room.Width / 2, room.Height - 1),
            Direction.West => new Coordinates(0, room.Height / 2),
            Direction.East => new Coordinates(room.Width - 1, room.Height / 2),
            _ => new Coordinates(1, 1)
        };
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

    public void NotifyRoomDoorsToggled()
    {
        // For now, all toggle doors in the current room have been notified by PressurePlate directly.
        // This method can do additional handling if needed.
    }


}