using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Items
{
    namespace TempleOfDoom.BusinessLogic.Models.Items
    {
        public class SankaraStone : IItem
        {
            public string Name => "Sankara Stone";
            public bool IsCollectible => true;

            public bool OnPlayerEnter(Player player, Room currentRoom)
            {
                player.CollectItem(this);
                return true;
            }
        }
    }
}
