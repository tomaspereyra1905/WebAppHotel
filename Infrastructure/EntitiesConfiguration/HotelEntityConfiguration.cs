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
    public class HotelEntityConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.ToTable("Hotel");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Stars).IsRequired();
            builder.Property(x => x.Address).IsRequired();
            builder.Property(x => x.PhoneNumber).IsRequired();
        }
    }
}
