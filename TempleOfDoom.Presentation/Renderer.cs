using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Models.Tile;
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

                    // If it's the player's position, render the player
                    if (coords.Equals(player.Position))
                    {
                        Console.ForegroundColor = ColorManager.GetColorForPlayer();
                        Console.Write("X ");
                        Console.ResetColor();
                    }
                    else
                    {
                        var tile = room.GetTileAt(coords);

                        if (tile is DoorTile doorTile)
                        {
                            // It's a door
                            var doorColor = doorTile.GetDoorColor();
                            Console.ForegroundColor = doorColor;
                            Console.Write($"{doorTile.Representation} ");
                            Console.ResetColor();
                        }
                        else if (tile is FloorTile floorTile)
                        {
                            // It's a floor tile; check for an item on it
                            if (floorTile.Item != null)
                            {
                                Console.ForegroundColor = ColorManager.GetColorForItem(floorTile.Item);
                                Console.Write($"{floorTile.Item.Representation} ");
                                Console.ResetColor();
                            }
                            else
                            {
                                // Plain floor tile
                                Console.ForegroundColor = ColorManager.GetColorForTile(floorTile);
                                Console.Write($"{floorTile.Representation} ");
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            // Some other tile (e.g., WallTile)
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

        public static void RenderWinScreen()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("****************************************************");
            Console.WriteLine("                   YOU WIN!                         ");
            Console.WriteLine("You have collected all the Sankara Stones!          ");
            Console.WriteLine("****************************************************");
            Console.ResetColor();
        }

        public static void RenderLoseScreen()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("****************************************************");
            Console.WriteLine("                   YOU LOSE!                        ");
            Console.WriteLine("You ran out of lives. Better luck next time.        ");
            Console.WriteLine("****************************************************");
            Console.ResetColor();
        }
    }
}
