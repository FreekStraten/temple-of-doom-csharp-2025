using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Helpers;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Models
{
    public class Player
    {
        public Coordinates Position { get; private set; }
        public int Lives { get; set; }
        public List<IItem> Inventory { get; } = new List<IItem>();

        public Player(int x, int y, int lives = 3)
        {
            Position = new Coordinates(x, y);
            Lives = lives;
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
            // Check boundaries
            if (newPosition.X < 0 || newPosition.X >= currentRoom.Width ||
                newPosition.Y < 0 || newPosition.Y >= currentRoom.Height)
            {
                // Can't move inside this room; might trigger AttemptRoomTransition if standing on a door.
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
            }
        }
    }
}