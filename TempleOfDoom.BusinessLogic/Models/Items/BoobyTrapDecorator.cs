using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Item.Item
{

    public class BoobyTrapDecorator : ItemDecorator
    {
        private readonly int _damage;

        public BoobyTrapDecorator(IItem wrappedItem, int damage) : base(wrappedItem)
        {
            _damage = damage;
        }

        public override string Name => "BoobyTrap"; // or combine with base, your choice
        public override string Representation => "O"; // or "B" if you prefer

        // It's still not collectible
        public override bool IsCollectible => false;

        public override bool OnPlayerEnter(Player player, Room currentRoom)
        {
            // Subtract lives
            player.Lives -= _damage;

            // Then delegate to the underlying item chain if you want
            // or just skip. Typically, if the underlying "base" is
            // empty, it won't do anything anyway:
            bool baseResult = base.OnPlayerEnter(player, currentRoom);

            // This does NOT remove itself (unless you also want “disappear”)
            return false;
        }
    }


}
