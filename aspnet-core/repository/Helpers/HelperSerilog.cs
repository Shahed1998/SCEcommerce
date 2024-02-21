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
                                            .WriteTo.File(
                                                    path: Path.Combine(Directory.GetCurrentDirectory(), "bin", "Logs", "Exceptions", "E-.log"),
                                                    rollingInterval: RollingInterval.Day)
                                            .CreateLogger();
        }

        public static void LogException(Exception ex)
        {
            verbose?.Error(ex, "An exception occured");
        }
    }
}
