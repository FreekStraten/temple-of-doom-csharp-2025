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
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "TempleOfDoom.json");
            JsonLevelLoader loader = new JsonLevelLoader();
            LevelDto levelData = loader.LoadLevel(filePath);

            Dictionary<int, Room> roomsById = new Dictionary<int, Room>();
            foreach (var roomDto in levelData.Rooms)
            {
                Room room = LevelMapper.MapRoomDtoToRoom(roomDto);
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
    }

}