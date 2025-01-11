using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Struct;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.DataAccess;
using CODE_TempleOfDoom_DownloadableContent;
using TempleOfDoom.BusinessLogic.Adapters;

namespace TempleOfDoom.BusinessLogic.Models
{
    public class Room
    {
        private List<IDoor> _doors = new List<IDoor>();

        // Instead of storing EnemyDto, we store real Enemy objects from the DLL.
        public List<Enemy> Enemies { get; } = new List<Enemy>();

        public int Id { get; set; }
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // Your original tiles
        public ITile[,] Layout { get; private set; }

        // A parallel array of FieldAdapters (so each tile corresponds to one FieldAdapter).
        // This allows enemies to access the grid via IField.
        public FieldAdapter[,] FieldAdapters { get; private set; }

        public Room(int width, int height)
        {
            Width = width;
            Height = height;
            // We now create both Layout and FieldAdapters in GenerateLayout().
        }

        public ITile GetTileAt(Coordinates coordinates)
        {
            return Layout[coordinates.Y, coordinates.X];
        }

        /// <summary>
        /// Fills the Layout with walls on the outer boundary, floor tiles inside,
        /// and creates matching FieldAdapters for each cell.
        /// </summary>
        public void GenerateLayout()
        {
            Layout = new ITile[Height, Width];
            FieldAdapters = new FieldAdapter[Height, Width];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    bool isWall = (x == 0 || y == 0 || x == Width - 1 || y == Height - 1);
                    if (isWall)
                    {
                        Layout[y, x] = new WallTile();
                    }
                    else
                    {
                        Layout[y, x] = new FloorTile();
                    }

                    // Create a FieldAdapter for the (x,y) cell that wraps this room & tile
                    FieldAdapter adapter = new FieldAdapter(this, x, y);
                    FieldAdapters[y, x] = adapter;
                }
            }
        }

        /// <summary>
        /// Places an item on the given FloorTile (if it’s actually a FloorTile).
        /// </summary>
        public void PlaceItem(Coordinates position, IItem item)
        {
            if (Layout[position.Y, position.X] is FloorTile floorTile)
            {
                floorTile.Item = item;
            }
        }

        /// <summary>
        /// Removes an item (if any) from a FloorTile at the given position.
        /// </summary>
        public void RemoveItemAt(Coordinates position)
        {
            if (Layout[position.Y, position.X] is FloorTile floorTile)
            {
                floorTile.Item = null;
            }
        }

        public void RegisterDoor(IDoor door)
        {
            if (!_doors.Contains(door))
            {
                _doors.Add(door);
            }
        }

        public IEnumerable<IDoor> GetDoors() => _doors;

        public IEnumerable<Coordinates> GetEnemyPositions()
        {
            // For each Enemy, create a Coordinates for its current location:
            return Enemies.Select(e => new Coordinates(e.CurrentXLocation, e.CurrentYLocation));
        }
    }
}
