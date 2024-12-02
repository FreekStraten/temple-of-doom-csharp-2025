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

            // Render the room
            Renderer.RenderRoom(firstRoom, player);

            // Additional game logic
        }
    }
}