using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Struct;
using TempleOfDoom.DataAccess;

namespace TempleOfDoom.Presentation
{
    public static class Renderer
    {
        public static void RenderRoom(Room room, Player player)
        {
            // Pre-fetch enemy positions to avoid repeating LINQ calls
            var enemyPositions = room.GetEnemyPositions().ToList();

            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    Coordinates coords = new Coordinates(x, y);

                    // 1) Check if it's the player
                    if (coords.Equals(player.Position))
                    {
                        Console.ForegroundColor = ColorManager.GetColorForPlayer();
                        Console.Write("X ");
                        Console.ResetColor();
                        continue;
                    }

                    // 2) Check if an enemy is here
                    bool isEnemyHere = enemyPositions.Any(pos => pos.X == x && pos.Y == y);
                    if (isEnemyHere)
                    {
                        // We don't care *what type* of enemy—just print 'E'
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("E ");
                        Console.ResetColor();
                        continue;
                    }

                    // 3. Tile rendering
                    var tile = room.GetTileAt(coords);
                    if (tile is DoorTile doorTile)
                    {
                        var doorColor = doorTile.GetDoorColor();
                        Console.ForegroundColor = doorColor;
                        Console.Write($"{doorTile.Representation} ");
                        Console.ResetColor();
                    }
                    else if (tile is FloorTile floorTile)
                    {
                        if (floorTile.Item != null)
                        {
                            Console.ForegroundColor = ColorManager.GetColorForItem(floorTile.Item);
                            Console.Write($"{floorTile.Item.Representation} ");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ColorManager.GetColorForTile(floorTile);
                            Console.Write($"{floorTile.Representation} ");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        // WallTile or other types
                        Console.ForegroundColor = ColorManager.GetColorForTile(tile);
                        Console.Write($"{tile.Representation} ");
                        Console.ResetColor();
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
