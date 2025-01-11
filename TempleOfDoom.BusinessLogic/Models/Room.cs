using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Struct;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.DataAccess;

namespace TempleOfDoom.BusinessLogic.Models
{
    public class Room
    {
        private List<IDoor> _doors = new List<IDoor>();

        public List<EnemyDto> Enemies { get; private set; } = new List<EnemyDto>();

        public int Id { get; set; }
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // 2D array of tiles
        public ITile[,] Layout { get; private set; }

        public Room(int width, int height)
        {
            Width = width;
            Height = height;
            Layout = new ITile[Height, Width];
        }

        public ITile GetTileAt(Coordinates coordinates)
            => Layout[coordinates.Y, coordinates.X];

        /// <summary>
        /// Fills the Layout with walls on the outer boundary,
        /// and floor tiles inside.
        /// </summary>
        public void GenerateLayout()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    bool isWall =
                        (x == 0 || y == 0 || x == Width - 1 || y == Height - 1);

                    Layout[y, x] = isWall
                        ? (ITile)new WallTile()
                        : new FloorTile(); // each FloorTile can hold an IItem if desired
                }
            }
        }

        /// <summary>
        /// Places an item on the given FloorTile (if it’s actually a FloorTile).
        /// </summary>
        public void PlaceItem(Coordinates position, IItem item)
        {
            // Only set the item if the tile is a FloorTile
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
            // Keep track of any door object in this room for toggling, etc.
            if (!_doors.Contains(door))
            {
                _doors.Add(door);
            }
        }

        public IEnumerable<IDoor> GetDoors() => _doors;
    }
}
