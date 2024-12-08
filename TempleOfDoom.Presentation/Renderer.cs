using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.Presentation
{
    public static class Renderer
    {
        public static void RenderRoom(Room room, Player player)
        {
            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    var coords = new Coordinates(x, y);
                    if (coords.Equals(player.Position))
                    {
                        // Player character
                        Console.ForegroundColor = ColorManager.GetColorForPlayer();
                        Console.Write("X ");
                        Console.ResetColor();
                    }
                    else
                    {
                        var tile = room.GetTileAt(coords);

                        // Check if there's an item on this tile
                        if (tile is ItemTileDecorator itemTile)
                        {
                            Console.ForegroundColor = ColorManager.GetColorForItem(itemTile.Item);
                            Console.Write($"{itemTile.Representation} ");
                            Console.ResetColor();
                        }
                        else
                        {
                            // Regular tile
                            Console.ForegroundColor = ColorManager.GetColorForTile(tile);
                            Console.Write($"{tile.Representation} ");
                            Console.ResetColor();
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        public static void RenderPlayerStatus(Player player, Room currentRoom)
        {
            Console.WriteLine();
            Console.WriteLine($"Current Room: {currentRoom.Id}");
            Console.WriteLine($"Lives: {player.Lives}");
            Console.Write("Inventory: ");
            if (player.Inventory.Count == 0)
            {
                Console.WriteLine("Empty");
            }
            else
            {
                foreach (var item in player.Inventory)
                {
                    Console.ForegroundColor = ColorManager.GetColorForItem(item);
                    Console.Write($"{item.Name}, ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
    }
}
