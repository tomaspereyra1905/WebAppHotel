﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IRoomService
    {
        List<Room> GetRooms();
        Task<IEnumerable<Room>> GetRoomsAsync();
    }
}
