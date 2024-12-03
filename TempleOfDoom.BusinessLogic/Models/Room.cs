using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Tile;

namespace TempleOfDoom.BusinessLogic.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();

        public ITile[,] Layout { get; private set; }

        public Room(int width, int height)
        {
            Width = width;
            Height = height;
            Layout = new ITile[height, width];
        }

        public void GenerateLayout()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                    {
                        Layout[y, x] = new WallTile();
                    }
                    else
                    {
                        Layout[y, x] = new FloorTile();
                    }
                }
            }
        }

    }
}
