using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Back_End.Data
{
    public class ApplicationDBContext : IdentityDbContext<UserModel>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<ItemModel> Items { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }

        public DbSet<UserFavoriteItem> UserFavoriteItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserFavoriteItem>(x => x.HasKey(p => new { p.UserId, p.ItemId }));

            builder.Entity<UserFavoriteItem>()
                .HasOne(u => u.User)
                .WithMany(u => u.UserFavoriteItems)
                .HasForeignKey(p => p.UserId);

            builder.Entity<UserFavoriteItem>()
                .HasOne(u => u.Item)
                .WithMany(u => u.UserFavoriteItems)
                .HasForeignKey(p => p.ItemId);


            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }

}