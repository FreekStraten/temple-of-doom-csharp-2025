using TempleOfDoom.BusinessLogic;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Struct;

public class GameService
{
    private Dictionary<int, Room> _roomsById;
    private Dictionary<int, Dictionary<Direction, int>> _roomConnections;

    public Room CurrentRoom { get; private set; }
    public Player Player { get; private set; }

    public GameService(Room currentRoom, Player player, Dictionary<int, Room> roomsById, Dictionary<int, Dictionary<Direction, int>> roomConnections)
    {
        CurrentRoom = currentRoom;
        Player = player;
        _roomsById = roomsById;
        _roomConnections = roomConnections;
    }

    public void HandlePlayerMovement(Direction direction)
    {
        // First attempt to move the player within the current room.
        bool moved = Player.TryMove(direction, CurrentRoom);

        if (!moved)
        {
            // The player could not move within the room. This might mean they are on a door tile
            // and trying to move out of the room. Let's check if they can transition to another room.
            ITile currentTile = CurrentRoom.GetTileAt(Player.Position);
            if (currentTile is DoorTile)
            {
                // Player is standing on a door tile and tried to move out of the room.
                // Attempt a room transition in the given direction.
                if (AttemptRoomTransition(direction))
                {
                    // Transition successful, player is now in the next room
                    return;
                }
            }
            // If we get here, either it wasn't a door, or no connection in that direction,
            // so the player remains in place.
        }
        else
        {
            // Player moved successfully inside the room.
            // Check if they landed on an item and trigger item effects.
            var tile = CurrentRoom.GetTileAt(Player.Position);

            if (tile is ItemTileDecorator itemTile)
            {
                bool shouldRemove = itemTile.Item.OnPlayerEnter(Player);
                if (shouldRemove)
                {
                    CurrentRoom.RemoveItemAt(Player.Position);
                }
            }
        }
    }

    /// <summary>
    /// Attempts to transition the player to another room in the given direction.
    /// Returns true if successful, false otherwise.
    /// </summary>
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

    /// <summary>
    /// Returns the position of the door tile in the next room corresponding to the direction we came from.
    /// </summary>
    private Coordinates GetEntryPositionInNextRoom(Room nextRoom, Direction comingFromDirection)
    {
        int doorX, doorY;
        switch (comingFromDirection)
        {
            case Direction.North:
                doorX = nextRoom.Width / 2;
                doorY = 0; // top edge door
                break;
            case Direction.South:
                doorX = nextRoom.Width / 2;
                doorY = nextRoom.Height - 1; // bottom edge door
                break;
            case Direction.West:
                doorX = 0; // left edge door
                doorY = nextRoom.Height / 2;
                break;
            case Direction.East:
                doorX = nextRoom.Width - 1; // right edge door
                doorY = nextRoom.Height / 2;
                break;
            default:
                // Fallback, though we should never hit this
                doorX = 1;
                doorY = 1;
                break;
        }

        return new Coordinates(doorX, doorY);
    }
}
