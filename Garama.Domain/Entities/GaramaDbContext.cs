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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}

