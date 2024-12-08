using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Struct;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Decorators;

namespace TempleOfDoom.BusinessLogic.Models
{
    public class Room
    {
        private List<IDoor> _doors = new List<IDoor>();
        public int Id { get; set; }
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ITile[,] Layout { get; private set; }

        public Room(int width, int height)
        {
            Width = width;
            Height = height;
            Layout = new ITile[Height, Width];
        }

        public ITile GetTileAt(Coordinates coordinates) => Layout[coordinates.Y, coordinates.X];

        public void GenerateLayout()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Layout[y, x] = (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                        ? (ITile)new WallTile()
                        : new FloorTile();
                }
            }
        }

        public void PlaceItem(Coordinates position, IItem item)
        {
            Tile.Tile baseTile = (Tile.Tile)Layout[position.Y, position.X];
            Layout[position.Y, position.X] = new ItemTileDecorator(baseTile, item);
        }

        public void RemoveItemAt(Coordinates position)
        {
            var tile = Layout[position.Y, position.X] as ItemTileDecorator;
            if (tile != null)
            {
                Layout[position.Y, position.X] = tile.GetBaseTile();
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
    }
}