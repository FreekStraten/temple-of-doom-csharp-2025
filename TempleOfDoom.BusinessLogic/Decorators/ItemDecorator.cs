using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models;

namespace TempleOfDoom.BusinessLogic.Decorators
{
    public abstract class ItemDecorator : IItem
    {
        protected IItem _wrappedItem;

        protected ItemDecorator(IItem wrappedItem)
        {
            _wrappedItem = wrappedItem;
        }

        public virtual string Name => _wrappedItem.Name;
        public virtual string Representation => _wrappedItem.Representation;
        public virtual bool IsCollectible => _wrappedItem.IsCollectible;

        public virtual bool OnPlayerEnter(Player player, Room currentRoom)
        {
            // By default, just delegate to the wrapped item
            return _wrappedItem.OnPlayerEnter(player, currentRoom);
        }
    }

}
