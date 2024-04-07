using System.IO;
using Microsoft.Extensions.Configuration;

namespace entity.DataContext.Configurations
{
    public class AppConfiguration
    {
        public string? sqlConnectionString { get; set; }

        public AppConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configBuilder.AddJsonFile(path, false);
            var root = configBuilder.Build();
            var appSettings = root.GetSection("ConnectionStrings:DefaultConnection");
            sqlConnectionString = appSettings.Value;
        }
    }
}
