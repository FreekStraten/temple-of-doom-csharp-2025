using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Item.Item
{
    public class DisappearingBoobyTrap : IItem
    {
        public int Damage { get; }
        public string Name => "Disappearing BoobyTrap";
        public bool IsCollectible => false;

        public DisappearingBoobyTrap(int damage)
        {
            Damage = damage;
        }

        public bool OnPlayerEnter(Player player, Room currentRoom)
        {
            player.Lives -= Damage;
            return true; 
        }
    }
}

