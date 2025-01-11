using CODE_TempleOfDoom_DownloadableContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Adapters
{
    public class FieldAdapter : IField
    {
        private readonly Room _room;
        private readonly int _x;
        private readonly int _y;

        public bool CanEnter => IsFloorOrOpenDoor();

        // The DLL expects this to get/set the occupant (an Enemy or null).
        // If an enemy moves onto this tile, .Item = that enemy.
        public IPlacable Item { get; set; }

        public FieldAdapter(Room room, int x, int y)
        {
            _room = room;
            _x = x;
            _y = y;
        }

        /// <summary>
        /// direction: 0 (north), 1 (east), 2 (south), 3 (west).
        /// </summary>
        public IField GetNeighbour(int direction)
        {
            int dx = 0, dy = 0;
            switch (direction)
            {
                case 0: dy = -1; break; // north
                case 1: dx = +1; break; // east
                case 2: dy = +1; break; // south
                case 3: dx = -1; break; // west
            }

            int nx = _x + dx;
            int ny = _y + dy;

            // Check room bounds
            if (nx < 0 || ny < 0 || nx >= _room.Width || ny >= _room.Height)
            {
                return null;
            }

            // Return the neighbor's adapter
            return _room.FieldAdapters[ny, nx];
        }

        private bool IsFloorOrOpenDoor()
        {
            var tile = _room.GetTileAt(new Coordinates(_x, _y));

            // If it's a FloorTile => passable
            if (tile is FloorTile) return true;

            // If it's a DoorTile => you can decide whether enemies can pass if the door is open
            if (tile is DoorTile doorTile)
            {
                // By default, let's say enemies cannot pass closed doors:
                // If you want enemies to be blocked by doors, return doorTile.Door.IsOpen(...) 
                // But you might not have a "player" to pass. 
                // We’ll assume “always blocked” for simplicity:
                return false;
            }

            // If it's a WallTile or anything else => not passable
            return false;
        }
    }
}
