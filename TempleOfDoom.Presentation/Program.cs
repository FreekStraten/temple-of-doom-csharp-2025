using TempleOfDoom.BusinessLogic;
using TempleOfDoom.BusinessLogic.Enum;
using TempleOfDoom.BusinessLogic.Mappers;
using TempleOfDoom.BusinessLogic.Models;
using TempleOfDoom.DataAccess;

namespace TempleOfDoom.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Path to the JSON file
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "TempleOfDoom.json");

            // Load and parse data
            JsonLevelLoader loader = new JsonLevelLoader();
            LevelDto levelData = loader.LoadLevel(filePath);

            // Use the mapper to convert RoomDto to Room
            Room firstRoom = LevelMapper.MapRoomDtoToRoom(levelData.Rooms[0]);
            Player player = LevelMapper.MapPlayerDtoToPlayer(levelData.Player);

            // Create GameService instance
            GameService gameService = new GameService(firstRoom, player);

            // Game loop
            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();

                // Render the room
                Renderer.RenderRoom(firstRoom, player);

                // Get user input
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                Direction? direction = InputHandler.GetDirectionFromInput(keyInfo.Key);

                if (direction.HasValue)
                {
                    // Delegate movement handling to GameService
                    gameService.HandlePlayerMovement(direction.Value);
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isRunning = false;
                }
            }
        }
    }
}