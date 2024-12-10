using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using TempleOfDoom.BusinessLogic;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Factories;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Mappers;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.DataAccess;
using TempleOfDoom.Presentation.RemoveLater;

namespace TempleOfDoom.Presentation
{
    public class Program
    {
        private static bool StartGame = true;

        public static void Main(string[] args)
        {
            if (StartGame)
            {

                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "TempleOfDoom.json");
                JsonLevelLoader loader = new JsonLevelLoader();
                LevelDto levelData = loader.LoadLevel(filePath);

                IItemFactory itemFactory = new DefaultItemFactory();

                Dictionary<int, Room> roomsById = new Dictionary<int, Room>();
                foreach (var roomDto in levelData.Rooms)
                {
                    Room room = LevelMapper.MapRoomDtoToRoom(roomDto, itemFactory);
                    roomsById[room.Id] = room;
                }

                LevelMapper.ApplyConnectionsToRooms(levelData.Connections, roomsById);
                var roomConnections = LevelMapper.CreateRoomConnectionMap(levelData.Connections);

                Player player = LevelMapper.MapPlayerDtoToPlayer(levelData.Player);
                Room currentRoom = roomsById[levelData.Player.StartRoomId];

                GameService gameService = new GameService(currentRoom, player, roomsById, roomConnections);

                bool isRunning = true;
                while (isRunning)
                {
                    if (gameService.IsGameOver)
                    {
                        // Game ended (win or lose)
                        Console.Clear();
                        if (gameService.IsWin)
                        {
                            Renderer.RenderWinScreen();
                        }
                        else if (gameService.IsLose)
                        {
                            Renderer.RenderLoseScreen();
                        }
                        break;
                    }

                    Console.Clear();
                    Renderer.RenderRoom(gameService.CurrentRoom, gameService.Player);
                    Renderer.RenderPlayerStatus(gameService.Player, gameService.CurrentRoom);

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    Direction? direction = InputHandler.GetDirectionFromInput(keyInfo.Key);

                    if (direction.HasValue)
                    {
                        gameService.HandlePlayerMovement(direction.Value);
                    }
                    else if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        isRunning = false;
                    }
                }

                // Wait for a key press before closing
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(true);
            }
            else
            {
                ExtractAndOpenCode();
            }
        }

        private static void ExtractAndOpenCode()
        {
            // Define the project root path. Adjust this path as needed to get to the solution folder
            string projectRootPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", ".."));

            // Define the output file path. For example, place it in a "Output" folder within the project directory.
            string outputFilePath = Path.Combine(projectRootPath, "current_code.txt");

            // Use the CodeExtractor class to perform the extraction
            string[] includedDirectories = new string[] { "TempleOfDoom.Presentation", "TempleOfDoom.BusinessLogic", "TempleOfDoom.DataAccess" };
            CodeExtractor.ExtractCodeToTxt(projectRootPath, outputFilePath, includedDirectories);

            // Use the FileOpener class to open the output file
            FileOpener.OpenFile(outputFilePath);
        }
    }
}