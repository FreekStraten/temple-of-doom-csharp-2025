using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Item.Item
{
    public class KeyDecorator : ItemDecorator
    {
        private readonly string _color;

        public KeyDecorator(IItem wrappedItem, string color)
            : base(wrappedItem)
        {
            _color = color;
        }

        public string Color => _color;  // <-- ADD THIS PROPERTY

        public override string Name => $"Key({_color})";
        public override string Representation => "K";
        public override bool IsCollectible => true;

        public override bool OnPlayerEnter(Player player, Room currentRoom)
        {
            player.CollectItem(this);
            return true; // remove from the floor
        }
    }
}
