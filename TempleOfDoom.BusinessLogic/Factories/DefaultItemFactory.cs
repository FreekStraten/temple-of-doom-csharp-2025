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
            return itemDto.Type.ToLower() switch
            {
                "sankara stone" => new SankaraStone(),
                "key" => new KeyItem(itemDto.Color),
                "boobytrap" => new BoobyTrap(itemDto.Damage ?? 1),
                "disappearing boobytrap" => new DisappearingBoobyTrap(itemDto.Damage ?? 1),
                "pressure plate" => new PressurePlate(),
                _ => throw new ArgumentException($"Unknown item type: {itemDto.Type}")
            };
        }
    }
}
