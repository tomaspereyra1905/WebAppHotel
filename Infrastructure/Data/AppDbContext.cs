using Core.Entities;
using Infrastructure.EntitiesConfiguration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new HotelEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RoomEntityConfiguration());
            modelBuilder.ApplyConfiguration(new HotelRoomEntityConfiguration());
        }
    }
}
