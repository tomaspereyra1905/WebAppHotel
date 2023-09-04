using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepository)
        {
                _roomRepository = roomRepository;
        }
        public List<Room> GetRooms()
        {
            return _roomRepository.GetRooms();
        }
    }
}
