using TempleOfDoom.BusinessLogic;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Models.Items;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Struct;

public class GameService
{
    private Dictionary<int, Room> _roomsById;
    private Dictionary<int, Dictionary<Direction, int>> _roomConnections;

    private int _totalStones;
    private int _collectedStones;

    public Room CurrentRoom { get; private set; }
    public Player Player { get; private set; }

    public bool IsWin { get; private set; }
    public bool IsLose { get; private set; }

    public bool IsGameOver => IsWin || IsLose;

    public GameService(Room currentRoom, Player player, Dictionary<int, Room> roomsById, Dictionary<int, Dictionary<Direction, int>> roomConnections)
    {
        CurrentRoom = currentRoom;
        Player = player;
        _roomsById = roomsById;
        _roomConnections = roomConnections;

        CountTotalStones();
    }

    private void CountTotalStones()
    {
        _totalStones = 0;
        foreach (var room in _roomsById.Values)
        {
            // Count Sankara Stones by checking for ItemTileDecorators with "Sankara Stone"
            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    if (room.Layout[y, x] is ItemTileDecorator itemTile && itemTile.Item is SankaraStone)
                    {
                        _totalStones++;
                    }
                }
            }
        }
    }

    public void HandlePlayerMovement(Direction direction)
    {
        bool moved = Player.TryMove(direction, CurrentRoom);

        if (!moved)
        {
            // Check if the attempted direction leads outside the current room
            if (IsMoveOutsideRoom(direction))
            {
                ITile currentTile = CurrentRoom.GetTileAt(Player.Position);
                if (currentTile is DoorTile)
                {
                    // Only attempt room transition if we actually have a corresponding connection in that direction
                    if (AttemptRoomTransition(direction))
                    {
                        return;
                    }
                }
            }
            // If we get here, no valid room transition occurred.
            // This means the player can't move in that direction and is stuck at current position.
        }
        else
        {
            // Player moved successfully inside the room.
            // Check if they landed on an item and trigger item effects.
            var tile = CurrentRoom.GetTileAt(Player.Position);

            if (tile is ItemTileDecorator itemTile)
            {
                bool shouldRemove = itemTile.Item.OnPlayerEnter(Player);

                // Check for lose condition (player's lives <= 0)
                if (Player.Lives <= 0)
                {
                    IsLose = true;
                    return;
                }

                // Check for sankara stone collection (win condition)
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
                    CurrentRoom.RemoveItemAt(Player.Position);
                }
            }
        }
    }

    /// <summary>
    /// Checks if moving in the specified direction would lead the player outside the current room boundaries.
    /// This helps ensure that we only consider a room transition when the player is actually trying to move through the door opening.
    /// </summary>
    private bool IsMoveOutsideRoom(Direction direction)
    {
        Coordinates nextPos = Player.GetNewPosition(direction);
        // If nextPos is outside room boundaries, that implies player is trying to move beyond this room's walls.
        if (nextPos.X < 0 || nextPos.X >= CurrentRoom.Width || nextPos.Y < 0 || nextPos.Y >= CurrentRoom.Height)
        {
            return true;
        }
        return false;
    }

    private bool AttemptRoomTransition(Direction direction)
    {
        if (_roomConnections.TryGetValue(CurrentRoom.Id, out var connectionsForRoom))
        {
            if (connectionsForRoom.TryGetValue(direction, out int nextRoomId))
            {
                Room nextRoom = _roomsById[nextRoomId];
                Player.UpdatePosition(GetEntryPositionInNextRoom(nextRoom, OppositeDirection(direction)));
                CurrentRoom = nextRoom;
                return true;
            }
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
        int doorX, doorY;
        switch (comingFromDirection)
        {
            case Direction.North:
                doorX = nextRoom.Width / 2;
                doorY = 0;
                break;
            case Direction.South:
                doorX = nextRoom.Width / 2;
                doorY = nextRoom.Height - 1;
                break;
            case Direction.West:
                doorX = 0;
                doorY = nextRoom.Height / 2;
                break;
            case Direction.East:
                doorX = nextRoom.Width - 1;
                doorY = nextRoom.Height / 2;
                break;
            default:
                doorX = 1;
                doorY = 1;
                break;
        }

        return new Coordinates(doorX, doorY);
    }
}
