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
            Console.WriteLine("You stepped on a pressure plate.");

            // Notify all toggleable doors in current room
            if (GameService.Instance != null && GameService.Instance.CurrentRoom != null)
            {
                foreach (var door in GameService.Instance.CurrentRoom.GetDoors())
                {
                    door.NotifyStateChange();
                }
            }

            return false;
        }
    }
}
