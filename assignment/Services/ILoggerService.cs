using Microsoft.Extensions.Logging;
using NLog;

namespace assignment.Services;

public interface ILoggerService
{
    void LogInfo(string message);

    void LogWarn(string message);

    void LogDebug(string message);

    void LogError(Exception ex);
}

public class LoggerService : ILoggerService
{
    private static NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

    public void LogDebug(string message)
    {
        _logger.Debug(message);
    }

    public void LogError(Exception ex)
    {
        _logger.Error(ex);
    }

    public void LogInfo(string message)
    {
        _logger.Info(message);
    }

    public void LogWarn(string message)
    {
        _logger.Warn(message);
    }
}