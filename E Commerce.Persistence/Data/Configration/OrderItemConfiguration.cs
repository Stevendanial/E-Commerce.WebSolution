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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.Price)
                .HasPrecision(8, 2);

            builder.OwnsOne(o => o.productItemOrdered, pi =>
            {
                pi.Property(p => p.ProductName).HasMaxLength(100);
                pi.Property(p => p.PictureUrl).HasMaxLength(200);
            });
        }
    }
}
