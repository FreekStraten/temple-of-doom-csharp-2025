using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Tile;

namespace TempleOfDoom.Presentation
{
    public static class ColorManager
    {
        // Default colors
        private static readonly ConsoleColor DefaultTileColor = ConsoleColor.White;
        private static readonly ConsoleColor DefaultItemColor = ConsoleColor.White;
        private static readonly ConsoleColor PlayerColor = ConsoleColor.Blue; // Example for the player

        // Dictionaries for quick lookups
        private static Dictionary<string, ConsoleColor> tileColors = new Dictionary<string, ConsoleColor>
        {
            // Map tile type names to colors
            { nameof(WallTile), ConsoleColor.Yellow },
            { nameof(FloorTile), ConsoleColor.Gray },
            { nameof(DoorTile), ConsoleColor.DarkGray }
            // Add more tile types here in the future
        };

        private static Dictionary<string, ConsoleColor> itemColors = new Dictionary<string, ConsoleColor>
        {
            // Map item "type" names or known names to colors
            // We'll handle keys separately since they contain their color in the name.
            { "Sankara Stone", ConsoleColor.DarkYellow }, // approximation for "orange"
            { "BoobyTrap", ConsoleColor.DarkRed },
            { "Disappearing BoobyTrap", ConsoleColor.Red },
            { "Pressure Plate", ConsoleColor.Cyan }
            // Keys will be handled dynamically
        };

        public static ConsoleColor GetColorForTile(ITile tile)
        {
            string tileTypeName = tile.GetType().Name;
            if (tileColors.ContainsKey(tileTypeName))
            {
                return tileColors[tileTypeName];
            }
            return DefaultTileColor;
        }

        public static ConsoleColor GetColorForItem(IItem item)
        {
            // Keys have color embedded in their name, e.g. "Key(green)", "Key(red)"
            if (item.Name.StartsWith("Key("))
            {
                return GetKeyColor(item.Name);
            }

            if (itemColors.ContainsKey(item.Name))
            {
                return itemColors[item.Name];
            }

            return DefaultItemColor;
        }

        public static ConsoleColor GetColorForPlayer()
        {
            return PlayerColor;
        }

        private static ConsoleColor GetKeyColor(string keyName)
        {
            // keyName format: "Key(color)"
            // Extract the color from between parentheses
            int start = keyName.IndexOf('(') + 1;
            int end = keyName.IndexOf(')');
            if (start > 0 && end > start)
            {
                string colorStr = keyName.Substring(start, end - start).ToLower();
                return colorStr switch
                {
                    "red" => ConsoleColor.Red,
                    "green" => ConsoleColor.Green,
                    "blue" => ConsoleColor.Blue, // If you ever have a blue key
                    _ => DefaultItemColor
                };
            }

            return DefaultItemColor;
        }
    }
}
