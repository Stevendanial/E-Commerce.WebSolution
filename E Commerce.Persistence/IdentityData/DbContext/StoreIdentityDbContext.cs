using E_Commerce.Domain.Entites.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.IdentityData.DbContext
{
    public  class StoreIdentityDbContext:IdentityDbContext<ApplicationUser>
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> option):base(option) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Address>().ToTable("Address");
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UsersRoles");

        }

    }
}
