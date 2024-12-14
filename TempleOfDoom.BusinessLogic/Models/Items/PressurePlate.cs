using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Items
{
    namespace TempleOfDoom.BusinessLogic.Models.Items
    {
        public class PressurePlate : IItem
        {
            public string Name => "Pressure Plate";
            public bool IsCollectible => false;

            public bool OnPlayerEnter(Player player, Room currentRoom) 
            {
                foreach (var door in currentRoom.GetDoors()) 
                {
                    door.NotifyStateChange();
                }
                return false;
            }
        }
    }

}
