using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;

namespace TempleOfDoom.BusinessLogic.Models.Tile
{
    public class DoorTile : Tile
    {
        private readonly IDoor _door;
        private readonly bool _isHorizontal;

        // Add a public property to expose the door
        public IDoor Door => _door;

        public DoorTile(IDoor door, bool isHorizontal)
        {
            _door = door;
            _isHorizontal = isHorizontal;
        }

        public override string Representation => _door.GetRepresentation(_isHorizontal).ToString();
        public override bool IsWalkable => false;

        public ConsoleColor GetDoorColor() => _door.GetColor();
    }
}
