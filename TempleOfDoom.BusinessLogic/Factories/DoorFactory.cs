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
        public static IDoor CreateDoor(DoorDto doorDto)
        {
            return doorDto.Type.ToLower() switch
            {
                "colored" => new ColoredDoor(doorDto.Color),
                "toggle" => new ToggleDoor(),
                "closing gate" => new ClosingGateDoor(),
                "open on odd" => new OpenOnOddDoor(),
                "open on stones in room" => new OpenOnStonesInRoomDoor(doorDto.NoOfStones ?? 0),
                _ => new DefaultDoor()
            };
        }

        public static IDoor CreateCompositeDoor(List<DoorDto> doorDtos)
        {
            if (doorDtos == null || doorDtos.Count == 0)
            {
                return new DefaultDoor();
            }

            if (doorDtos.Count == 1)
            {
                return CreateDoor(doorDtos[0]);
            }

            IDoor result = CreateDoor(doorDtos[0]);
            for (int i = 1; i < doorDtos.Count; i++)
            {
                IDoor next = CreateDoor(doorDtos[i]);
                result = new DoorDecorator(result, next);
            }
            return result;
        }
    }
}
