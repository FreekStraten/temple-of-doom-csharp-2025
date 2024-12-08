using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.BusinessLogic.Interfaces;
using TempleOfDoom.BusinessLogic.Models.Doors;
using TempleOfDoom.DataAccess;

namespace TempleOfDoom.BusinessLogic.Factories
{
    public static class DoorFactory
    {
        public static IDoor CreateDoor(DoorDto doorDto)
        {
            // Decide door type based on doorDto.Type
            // For now just handle a few cases:
            return doorDto.Type.ToLower() switch
            {
                "colored" => new ColoredDoor(doorDto.Color),
                "toggle" => new ToggleDoor(),
                "closing gate" => new ClosingGateDoor(),
                "open on odd" => new OpenOnOddDoor(),
                "open on stones in room" => new OpenOnStonesInRoomDoor(doorDto.NoOfStones ?? 0),
                // If no known type, default to a standard open door:
                _ => new DefaultDoor()
            };
        }
    }
}
