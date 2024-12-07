using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Item.Item;
using TempleOfDoom.BusinessLogic.Models.Items;
using TempleOfDoom.DataAccess;

namespace TempleOfDoom.BusinessLogic
{
    public static class ItemFactory
    {
        public static IItem CreateItem(ItemDto itemDto)
        {
            switch (itemDto.Type.ToLower())
            {
                case "sankara stone":
                    return new SankaraStone();
                case "key":
                    return new KeyItem(itemDto.Color);
                case "boobytrap":
                    return new BoobyTrap(itemDto.Damage ?? 1);
                case "disappearing boobytrap":
                    return new DisappearingBoobyTrap(itemDto.Damage ?? 1);
                case "pressure plate":
                    return new PressurePlate();
                default:
                    throw new ArgumentException($"Unknown item type: {itemDto.Type}");
            }
        }
    }
}
