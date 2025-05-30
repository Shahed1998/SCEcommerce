using Serilog;
using Serilog.Events;

namespace Utility.Helpers
{
    public static class HelperSerilog
    {
        public static ILogger _logger;
        public static string loggingDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");

        static HelperSerilog()
        {
            _logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                                .WriteTo.File(Path.Combine(loggingDir, "Information", "information-.log"),
                                      rollingInterval: RollingInterval.Month))
                            .WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                                .WriteTo.File(Path.Combine(loggingDir, "Error", "error-.log"),
                                      rollingInterval: RollingInterval.Day))
                            .CreateLogger();

            Log.Logger = _logger;
        }

        public static void LogError(string message = "", Exception? ex = null)
        {
            if (ex != null)
            {
                _logger.Error(message, ex);
            }
            else
            {
                _logger.Error(message);
            }
        }

        public static void LogInformation(string message = "", Exception? ex = null)
        {
            if (ex != null)
            {
                _logger.Information(message, ex);
            }
            else
            {
                _logger.Information(message);
            }
        }

        // Close and flush the logger
        public static void CloseAndFlushLogger()
        {
            Log.CloseAndFlush();
        }
    }
}
