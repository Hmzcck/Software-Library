using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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
    }

}