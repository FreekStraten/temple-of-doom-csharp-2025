using System.Text.Json;

namespace TempleOfDoom.DataAccess
{
    public class JsonLevelLoader
    {
        public LevelDto LoadLevel(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("JSON file not found.", filePath);
            }

            string jsonContent = File.ReadAllText(filePath);

            // Configure deserialization options
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Makes JSON property names case-insensitive
            };

            LevelDto levelData = JsonSerializer.Deserialize<LevelDto>(jsonContent, options);
            return levelData;
        }
    }
}
