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
        bool moved = Player.TryMove(direction, CurrentRoom);
        if (moved)
        {
            ITile currentTile = CurrentRoom.GetTileAt(Player.Position);
            if (currentTile is DoorTile)
            {
                AttemptRoomTransition(direction);
            }
        }
    }

    private void AttemptRoomTransition(Direction direction)
    {
        if (_roomConnections.TryGetValue(CurrentRoom.Id, out var connectionsForRoom))
        {
            if (connectionsForRoom.TryGetValue(direction, out int nextRoomId))
            {
                Room nextRoom = _roomsById[nextRoomId];
                Player.UpdatePosition(GetEntryPositionInNextRoom(nextRoom, OppositeDirection(direction)));
                CurrentRoom = nextRoom;
            }
        }
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
                doorX = nextRoom.Width / 2; doorY = 0; return new Coordinates(doorX, doorY + 1);
            case Direction.South:
                doorX = nextRoom.Width / 2; doorY = nextRoom.Height - 1; return new Coordinates(doorX, doorY - 1);
            case Direction.West:
                doorX = 0; doorY = nextRoom.Height / 2; return new Coordinates(doorX + 1, doorY);
            case Direction.East:
                doorX = nextRoom.Width - 1; doorY = nextRoom.Height / 2; return new Coordinates(doorX - 1, doorY);
            default:
                return new Coordinates(1, 1);
        }
    }
}
