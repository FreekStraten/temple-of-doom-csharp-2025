using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleOfDoom.BusinessLogic.Mappers
{
    using TempleOfDoom.BusinessLogic.Models;
    using TempleOfDoom.DataAccess;

    public static class LevelMapper
    {
        public static Room MapRoomDtoToRoom(RoomDto roomDto)
        {
            var room = new Room(roomDto.Width, roomDto.Height)
            {
                Id = roomDto.Id,
                Type = roomDto.Type
            };

            room.GenerateLayout();
            return room;
        }


        /*
        public static Item MapItemDtoToItem(ItemDto itemDto)
        {
            return new Item
            {
                // Map ItemDto properties here
                X = itemDto.X,
                Y = itemDto.Y
            };
        }
        */

        public static Player MapPlayerDtoToPlayer(PlayerDto playerDto)
        {
            //log playerDto.StartX, playerDto.StartY, playerDto.Lives to the console
            Console.WriteLine($"PlayerDto: StartX: {playerDto.StartX}, StartY: {playerDto.StartY}, Lives: {playerDto.Lives}");

            return new Player(playerDto.StartX, playerDto.StartY, playerDto.Lives);
        }
    }
}
