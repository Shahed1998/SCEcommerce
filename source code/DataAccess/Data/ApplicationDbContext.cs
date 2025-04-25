using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
           
        }

        public DbSet<Category> categories { get; set; } 
        public DbSet<Product> products { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
