using Serilog;

namespace repository.Helpers
{
    public static class HelperSerilog
    {
        static ILogger? verbose;

        static HelperSerilog()
        {
            verbose = new LoggerConfiguration()
                                            .MinimumLevel.Verbose()
                                            .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "bin", "Logs", "Verbose", "Verbose.log"), rollingInterval: RollingInterval.Day)
                                            .CreateLogger();
        }

        public static void LogException(Exception ex)
        {
            verbose?.Error(ex, "An exception occured");
        }


    }
}
