using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Items;

namespace TempleOfDoom.BusinessLogic.Models.Doors
{
    public class OpenOnStonesInRoomDoor : IDoor
    {
        private readonly int _requiredStones;
        public OpenOnStonesInRoomDoor(int requiredStones)
        {
            _requiredStones = requiredStones;
        }

        public char GetRepresentation(bool isHorizontal) => isHorizontal ? '=' : '|';
        public ConsoleColor GetColor() => ConsoleColor.Blue;

        public bool IsOpen(Player player, Room currentRoom)
        {
            int stonesInRoom = 0;
            for (int y = 0; y < currentRoom.Height; y++)
            {
                for (int x = 0; x < currentRoom.Width; x++)
                {
                    var tile = currentRoom.GetTileAt(new Struct.Coordinates(x, y));
                    if (tile is ItemTileDecorator itemDec && itemDec.Item is SankaraStone)
                    {
                        stonesInRoom++;
                    }
                }
            }

            return stonesInRoom == _requiredStones;
        }

        public void NotifyStateChange() { /* Not needed */ }
    }
}
