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
            // Load level data
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "TempleOfDoom.json");
            JsonLevelLoader loader = new JsonLevelLoader();
            LevelDto levelData = loader.LoadLevel(filePath);

            // Map all rooms first
            Dictionary<int, Room> roomsById = new Dictionary<int, Room>();
            foreach (var roomDto in levelData.Rooms)
            {
                Room room = LevelMapper.MapRoomDtoToRoom(roomDto);
                roomsById[room.Id] = room;
            }

            // Apply connections to place door tiles
            LevelMapper.ApplyConnectionsToRooms(levelData.Connections, roomsById);

            // Create player
            Player player = LevelMapper.MapPlayerDtoToPlayer(levelData.Player);

            // For now, we start in the player's start room
            Room currentRoom = roomsById[levelData.Player.StartRoomId];

            // Create GameService instance
            GameService gameService = new GameService(currentRoom, player);

            // Game loop
            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                Renderer.RenderRoom(currentRoom, player);

                // Get user input
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                Direction? direction = InputHandler.GetDirectionFromInput(keyInfo.Key);

                if (direction.HasValue)
                {
                    // Move player
                    bool moved = player.TryMove(direction.Value, currentRoom);

                    // In the future, if player stands on door tile and moves through it, 
                    // change `currentRoom` to the connected room.
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isRunning = false;
                }
            }
        }
    }
}