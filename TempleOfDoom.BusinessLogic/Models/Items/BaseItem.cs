using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Items
{
    public class BaseItem : IItem
    {
        public string Name { get; protected set; } = "BaseItem";
        public virtual string Representation { get; protected set; } = "?";
        public virtual bool IsCollectible { get; protected set; } = false;

        public BaseItem() { }
        public BaseItem(string name, string representation, bool isCollectible)
        {
            Name = name;
            Representation = representation;
            IsCollectible = isCollectible;
        }

        public virtual bool OnPlayerEnter(Player player, Room currentRoom)
        {
            // By default, do nothing; return false = do not remove the item
            return false;
        }
    }

}
