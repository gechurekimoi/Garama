using System;
using Microsoft.EntityFrameworkCore;

namespace Garama.Domain.Entities
{
    public class GaramaDbContext : DbContext
    {
        public GaramaDbContext(DbContextOptions<GaramaDbContext> options):base(options)
        {
            
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }     
        public DbSet<User> Users { get; set; }     
        public DbSet<Category> Categories { get; set; }     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}

