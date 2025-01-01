using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Items;
using TempleOfDoom.BusinessLogic.Models.Items.TempleOfDoom.BusinessLogic.Models.Items;
using TempleOfDoom.BusinessLogic.Struct;

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
            // Count the sankara stones in the current room
            int stonesInRoom = 0;
            for (int y = 0; y < currentRoom.Height; y++)
            {
                for (int x = 0; x < currentRoom.Width; x++)
                {
                    var tile = currentRoom.GetTileAt(new Coordinates(x, y));
                    if (tile is ItemTileDecorator itemTile &&
                        itemTile.Item is SankaraStone)
                    {
                        stonesInRoom++;
                    }
                }
            }

            // Door only opens if base door is open AND the exact number of stones matches
            return base.IsOpen(player, currentRoom) && (stonesInRoom == _requiredStones);
        }

        public override char GetRepresentation(bool isHorizontal)
        {
            return isHorizontal ? '=' : '|';
        }

        public override ConsoleColor GetColor()
        {
            return base.GetColor();
        }
    }

}
