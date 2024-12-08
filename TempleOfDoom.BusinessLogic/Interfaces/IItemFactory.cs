using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.DataAccess;

namespace TempleOfDoom.BusinessLogic.Interfaces
{
    public interface IItemFactory
    {
        IItem CreateItem(ItemDto itemDto);
    }
}
