using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Items
{
    public class SankaraStoneDecorator : ItemDecorator
    {
        public SankaraStoneDecorator(IItem wrappedItem)
            : base(wrappedItem)
        {
        }

        public override string Name => "Sankara Stone";
        public override string Representation => "S";
        public override bool IsCollectible => true;

        public override bool OnPlayerEnter(Player player, Room currentRoom)
        {
            // Collect the stone
            player.CollectItem(this);

            // Remove from the room once picked up
            return true;
        }
    }

}
