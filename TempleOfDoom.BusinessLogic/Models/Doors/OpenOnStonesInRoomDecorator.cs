using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Items;
using TempleOfDoom.BusinessLogic.Struct;
using TempleOfDoom.BusinessLogic.Models.Tile;
using TempleOfDoom.BusinessLogic.Decorators;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class OpenOnStonesInRoomDecorator : DoorDecorator
    {
        private readonly int _requiredStones;

        public OpenOnStonesInRoomDecorator(IDoor wrappedDoor, int requiredStones)
            : base(wrappedDoor)
        {
            _requiredStones = requiredStones;
        }

        public override bool IsOpen(Player player, Room currentRoom)
        {
            // Count the Sankara stones in the current room:
            int stonesInRoom = 0;
            for (int y = 0; y < currentRoom.Height; y++)
            {
                for (int x = 0; x < currentRoom.Width; x++)
                {
                    var tile = currentRoom.GetTileAt(new Coordinates(x, y));
                    // If the tile is a FloorTile AND 
                    // that tile's item is a SankaraStoneDecorator, increment count
                    if (tile is FloorTile floorTile
                        && floorTile.Item is SankaraStoneDecorator)
                    {
                        stonesInRoom++;
                    }
                }
            }

            // Door only opens if:
            // 1) The base (wrapped) door is also open,
            // 2) The number of stones in the room == _requiredStones
            return base.IsOpen(player, currentRoom) && (stonesInRoom == _requiredStones);
        }

        public override char GetRepresentation(bool isHorizontal)
        {
            return base.GetRepresentation(isHorizontal);
        }

        public override ConsoleColor GetColor()
        {
            return base.GetColor();
        }
    }
}
