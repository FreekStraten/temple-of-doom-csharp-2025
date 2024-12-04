using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Struct;

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
                    Coordinates currentCoordinates = new Coordinates(x, y);

                    if (currentCoordinates.Equals(player.Position))
                    {
                        Console.Write("P ");
                    }
                    else
                    {
                        Console.Write($"{room.GetTileAt(currentCoordinates).Representation} ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
