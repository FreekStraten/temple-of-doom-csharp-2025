using System;
using System.Collections.Generic;
using System.Linq;
using TempleOfDoom.BusinessLogic.Decorators; 
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Struct;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Models.Doors;

namespace TempleOfDoom.BusinessLogic.Services
{
    public class DoorService : IDoorService
    {
        // Check if door in the given direction can be opened
        public bool CanPassThroughDoor(Player player, Room room, Direction direction)
        {
            var doorPos = GetDoorPositionForDirection(room, direction);
            var doorTile = room.GetTileAt(doorPos) as DoorTile;
            if (doorTile == null) return true; // No door tile => passable

            return doorTile.Door.IsOpen(player, room);
        }

        public void AfterPassingDoor(Room room, Direction direction)
        {
            var doorPos = GetDoorPositionForDirection(room, direction);
            var doorTile = room.GetTileAt(doorPos) as DoorTile;

            // If the door is a "closing gate" decorator, then we call NotifyStateChange
            // so it can permanently close after the player passes through.
            if (doorTile != null && ContainsDoorOfType<ClosingGateDoorDecorator>(doorTile.Door))
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

        /// <summary>
        /// Recursively checks whether the given door is or contains 
        /// a decorator of type T somewhere in its wrapping chain.
        /// </summary>
        private bool ContainsDoorOfType<T>(IDoor door) where T : IDoor
        {
            // If the current door *is* T, we are done
            if (door is T) return true;

            // If it's a decorator, inspect its wrapped door
            if (door is DoorDecorator decorator)
            {
                return ContainsDoorOfType<T>(decorator.WrappedDoor);
            }

            // If it's neither T nor a decorator, there's nowhere else to look
            return false;
        }
    }
}
