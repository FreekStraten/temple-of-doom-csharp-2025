using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Item.Item
{
    public class DisappearingBoobyTrapDecorator : ItemDecorator
    {
        private readonly int _damage;

        public DisappearingBoobyTrapDecorator(IItem wrappedItem, int damage)
            : base(wrappedItem)
        {
            _damage = damage;
        }

        public override string Name => "Disappearing BoobyTrap";
        public override string Representation => "@";
        public override bool IsCollectible => false;

        public override bool OnPlayerEnter(Player player, Room currentRoom)
        {
            // Deal damage to player
            player.Lives -= _damage;

            // Then remove the item (return true => remove from room after stepping)
            base.OnPlayerEnter(player, currentRoom);
            return true;
        }
    }

}

