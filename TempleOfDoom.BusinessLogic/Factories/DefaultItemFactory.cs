using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Item.Item;
using TempleOfDoom.BusinessLogic.Models.Items;
using TempleOfDoom.DataAccess;

namespace TempleOfDoom.BusinessLogic.Factories
{
    public class DefaultItemFactory : IItemFactory
    {
        public IItem CreateItem(ItemDto itemDto)
        {
            // Start with an empty base item
            IItem baseItem = new BaseItem();

            switch (itemDto.Type.ToLower())
            {
                case "boobytrap":
                    return new BoobyTrapDecorator(baseItem, itemDto.Damage ?? 1);

                case "disappearing boobytrap":
                    // single class that does both damage + disappearing
                    return new DisappearingBoobyTrapDecorator(baseItem, itemDto.Damage ?? 1);

                case "key":
                    return new KeyDecorator(baseItem, itemDto.Color);

                case "sankara stone":
                    return new SankaraStoneDecorator(baseItem);

                case "pressure plate":
                    return new PressurePlateDecorator(baseItem);

                default:
                    throw new ArgumentException($"Unknown item type: {itemDto.Type}");
            }
        }
    }
}
