using entity.DataContext.Configurations;
using entity.general_entities;
using Microsoft.EntityFrameworkCore;

namespace entity.DataContext
{
    public class DatabaseContext : DbContext
    {
        #region Configurations
        public static OptionsBuild ops = new OptionsBuild();

        public class OptionsBuild
        {
            public OptionsBuild() 
            {
                settings = new AppConfiguration();
                opsBuilder = new DbContextOptionsBuilder<DatabaseContext> {};
                opsBuilder.UseSqlServer(settings.sqlConnectionString);
                dbOptions = opsBuilder.Options;
            }   

            public DbContextOptionsBuilder<DatabaseContext>? opsBuilder { get; set;}
            public DbContextOptions<DatabaseContext>? dbOptions { get; set;}
            private AppConfiguration? settings { get; set;}
        }
    
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}
        #endregion

        #region DbSets
        public DbSet<Category> categories { get; set;}
        #endregion

    }
}
