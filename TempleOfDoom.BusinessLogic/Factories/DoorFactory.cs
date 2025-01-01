using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Decorators;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Doors;
using TempleOfDoom.DataAccess;

namespace TempleOfDoom.BusinessLogic.Factories
{
    public static class DoorFactory
    {
        public static IDoor CreateCompositeDoor(List<DoorDto> doorDtos)
        {
            // If no door info, return a bare door
            if (doorDtos == null || doorDtos.Count == 0)
            {
                return new DefaultDoor();
            }

            // Start with the "plain" door
            IDoor doorChain = new DefaultDoor();

            // Decorate for each door type
            foreach (var dto in doorDtos)
            {
                doorChain = WrapDoor(doorChain, dto);
            }

            return doorChain;
        }

        private static IDoor WrapDoor(IDoor baseDoor, DoorDto doorDto)
        {
            switch (doorDto.Type.ToLower())
            {
                case "colored":
                    return new ColoredDoorDecorator(baseDoor, doorDto.Color);

                case "toggle":
                    return new ToggleDoorDecorator(baseDoor);

                case "closing gate":
                    return new ClosingGateDoorDecorator(baseDoor);

                case "open on odd":
                    return new OpenOnOddDoorDecorator(baseDoor);

                case "open on stones in room":
                    return new OpenOnStonesInRoomDecorator(baseDoor, doorDto.NoOfStones ?? 0);

                // If some unknown type, just return baseDoor or optionally throw
                default:
                    return baseDoor;
            }
        }
    }
}
