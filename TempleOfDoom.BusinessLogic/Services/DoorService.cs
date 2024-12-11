using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Doors;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Services
{
    public class DoorService : IDoorService
    {
        // Check if door in given direction can be opened
        public bool CanPassThroughDoor(Player player, Room room, Direction direction)
        {
            var doorPos = GetDoorPositionForDirection(room, direction);
            var tile = room.GetTileAt(doorPos) as DoorTile;
            if (tile == null) return true; // No door tile means open?

            return tile.Door.IsOpen(player, room);
        }

        public void AfterPassingDoor(Room room, Direction direction)
        {
            var doorPos = GetDoorPositionForDirection(room, direction);
            var doorTile = room.GetTileAt(doorPos) as DoorTile;
            if (doorTile != null && ContainsDoorOfType<ClosingGateDoor>(doorTile.Door))
            {
                doorTile.Door.NotifyStateChange();
            }
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

        private bool ContainsDoorOfType<T>(IDoor door) where T : IDoor
        {
            if (door is T) return true;
            if (door is DoorDecorator dec)
            {
                return ContainsDoorOfType<T>(dec.PrimaryDoor) || ContainsDoorOfType<T>(dec.SecondaryDoor);
            }
            return false;
        }
    }

}
