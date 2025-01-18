using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Struct;

namespace TempleOfDoom.BusinessLogic.Models.Tile
{
    public class LadderTile : Tile
    {
        public override string Representation => "H";     // or any symbol you want
        public override bool IsWalkable => true;          // Player/enemies can stand on it

        public int ConnectedRoomId { get; }
        public Coordinates TargetCoordinates { get; }

        public LadderTile(int connectedRoomId, Coordinates targetCoordinates)
        {
            ConnectedRoomId = connectedRoomId;
            TargetCoordinates = targetCoordinates;
        }
    }

}
