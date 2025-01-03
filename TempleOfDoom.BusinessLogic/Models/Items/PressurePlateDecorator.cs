using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Items
{
    public class PressurePlateDecorator : ItemDecorator
    {
        public PressurePlateDecorator(IItem wrappedItem)
            : base(wrappedItem)
        {
        }

        public override string Name => "Pressure Plate";
        public override string Representation => "T";
        public override bool IsCollectible => false;

        public override bool OnPlayerEnter(Player player, Room currentRoom)
        {
            // Toggle all togglable doors in the room
            foreach (var door in currentRoom.GetDoors())
            {
                door.NotifyStateChange();
            }

            // Let base do whatever it does 
            base.OnPlayerEnter(player, currentRoom);

            // Typically, pressure plates remain, so return false
            return false;
        }
    }
}
