using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Item.Item
{
    public class KeyItem : IItem
    {
        public string Color { get; }
        public string Name => $"Key({Color})";
        public bool IsCollectible => true;

        public KeyItem(string color)
        {
            Color = color;
        }

        public bool OnPlayerEnter(Player player)
        {
            player.CollectItem(this);
            return true; // Remove after collection
        }
    }
}
