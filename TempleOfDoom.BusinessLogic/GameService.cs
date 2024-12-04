using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Struct;

public class GameService
{
    private Room _currentRoom;
    private Player _player;

    public GameService(Room currentRoom, Player player)
    {
        _currentRoom = currentRoom;
        _player = player;
    }

    public void HandlePlayerMovement(Direction direction)
    {
        _player.TryMove(direction, _currentRoom);
    }


    // Other game logic methods can be added here
}
