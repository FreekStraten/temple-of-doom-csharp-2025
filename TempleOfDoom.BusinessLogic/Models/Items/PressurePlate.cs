using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Items
{
    public class PressurePlate : IItem
    {
        public string Name => "Pressure Plate";
        public bool IsCollectible => false;

        public bool OnPlayerEnter(Player player)
        {
            // Implement pressure plate logic here
            // For now, just display a message
            Console.WriteLine("You stepped on a pressure plate.");
            return false; // Do not remove
        }
    }
}
