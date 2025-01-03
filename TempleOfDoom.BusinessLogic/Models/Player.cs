using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Helpers;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Items;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Models
{
    public class Player
    {
        public Coordinates Position { get; private set; }
        public int Lives { get; set; }
        public List<IItem> Inventory { get; } = new List<IItem>();

        private readonly IGameStateManager _gameStateManager;

        public Player(int x, int y, int lives, IGameStateManager gameStateManager)
        {
            Position = new Coordinates(x, y);
            Lives = lives;
            _gameStateManager = gameStateManager;
        }

        public Coordinates GetNewPosition(Direction direction)
        {
            Coordinates movement = DirectionHelper.ToCoordinates(direction);
            return Position + movement;
        }

        public void UpdatePosition(Coordinates newPosition)
        {
            Position = newPosition;
        }

        public bool TryMove(Direction direction, Room currentRoom)
        {
            Coordinates newPosition = GetNewPosition(direction);
            if (newPosition.X < 0 || newPosition.X >= currentRoom.Width ||
                newPosition.Y < 0 || newPosition.Y >= currentRoom.Height)
            {
                // Outside room bounds
                return false;
            }
            if (!currentRoom.GetTileAt(newPosition).IsWalkable)
            {
                return false;
            }
            Position = newPosition;
            return true;
        }

        public void CollectItem(IItem item)
        {
            if (item.IsCollectible)
            {
                Inventory.Add(item);

                // If it's a Sankara Stone, let the GameStateManager know
                if (item is SankaraStoneDecorator)
                {
                    _gameStateManager.OnSankaraStoneCollected();
                }
            }
        }
    }
}
