using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Tile;

namespace TempleOfDoom.BusinessLogic
{
   public class ItemTileDecorator : Tile
    {
        private readonly Tile _baseTile;
        public IItem Item { get; }

        public ItemTileDecorator(Tile baseTile, IItem item)
        {
            _baseTile = baseTile;
            Item = item;
        }

        public override string Representation => GetItemRepresentation();
        public override bool IsWalkable => _baseTile.IsWalkable;

        private string GetItemRepresentation()
        {
            // Represent items with their first letter or a specific symbol
            return Item.Name switch
            {
                "Sankara Stone" => "S",
                "Key(green)" => "G",
                "Key(red)" => "R",
                "BoobyTrap" => "B",
                "Disappearing BoobyTrap" => "D",
                "Pressure Plate" => "P",
                _ => "?"
            };
        }

        public ITile GetBaseTile() => _baseTile;
    }
}
