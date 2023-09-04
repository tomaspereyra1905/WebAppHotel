using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntitiesConfiguration
{
    public class HotelRoomEntityConfiguration : IEntityTypeConfiguration<HotelRoom>
    {
        public void Configure(EntityTypeBuilder<HotelRoom> builder)
        {
            builder.ToTable("HotelRoom");
            builder.HasKey(hr => new { hr.HotelId, hr.RoomId });
            builder.HasOne(hr => hr.Hotel)
                .WithMany(h => h.HotelRooms)
                .HasForeignKey(hr => hr.HotelId);
            builder.HasOne(hr => hr.Room)
                .WithMany(r => r.HotelRooms)
                .HasForeignKey(hr => hr.RoomId);
        }

    }
}
