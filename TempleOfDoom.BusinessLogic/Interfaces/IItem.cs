using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Models;

namespace TempleOfDoom.BusinessLogic.Interfaces
{
    public interface IItem
    {
        string Name { get; }
        string Representation { get; }     // Let each item or decorator define how it looks
        bool IsCollectible { get; }
        bool OnPlayerEnter(Player player, Room currentRoom);
    }

}
