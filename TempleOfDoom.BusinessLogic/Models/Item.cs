using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.BusinessLogic.Models
{
    public abstract class Item
    {
        public string Type { get; protected set; }
        public int X { get; set; }
        public int Y { get; set; }

        // Abstract method for interaction
        public abstract void Interact(Player player);
    }
}
