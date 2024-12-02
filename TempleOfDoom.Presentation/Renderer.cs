using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Models;

namespace TempleOfDoom.Presentation
{
    public class Renderer
    {
        public static void RenderRoom(Room room, Player player)
        {
            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    if (x == player.X && y == player.Y)
                    {
                        Console.Write("P "); // Player
                    }
                    else if (room.Items.Any(item => item.X == x && item.Y == y))
                    {
                        Console.Write("I "); // Item
                    }
                    else
                    {
                        Console.Write(". "); // Empty space
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
