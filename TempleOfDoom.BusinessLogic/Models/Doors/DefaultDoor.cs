using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class DefaultDoor : IDoor
    {
        public virtual char GetRepresentation(bool isHorizontal) => ' ';
        public virtual ConsoleColor GetColor() => ConsoleColor.White;
        public virtual bool IsOpen(Player player, Room currentRoom) => true;
        public virtual void NotifyStateChange() { /* no-op */ }
    }
}
