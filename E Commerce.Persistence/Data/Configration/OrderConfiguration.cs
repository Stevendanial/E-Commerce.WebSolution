using E_Commerce.Domain.Entites.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Data.Configration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
      

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x=>x.Subtotal)
                .HasPrecision(8,2);

            builder.OwnsOne(o => o.Address, a =>
            {

                a.Property(p => p.FirstName).HasMaxLength(50);
                a.Property(p => p.LastName).HasMaxLength(50);
                a.Property(p => p.City).HasMaxLength(50);
                a.Property(p => p.Street).HasMaxLength(50);
                a.Property(p => p.Country).HasMaxLength(50);

            });
        }
    }
}
