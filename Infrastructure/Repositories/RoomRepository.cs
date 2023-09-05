using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        private readonly AppDbContext dbContext;
        public RoomRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Room> GetRooms()
        {
            return dbContext.Rooms
                .OrderBy(room => room.Id)
                .ToList();
        }
        public async Task<IEnumerable<Room>> GetRoomsAsync() => await dbContext.Rooms.OrderBy(room => room.Id).ToListAsync();
    }
}
