using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;

namespace OllamaClient.Services;

public enum LogType
{
	Normal,
	Error,
	Important,
	Success,
	Warning,
	Debug
}

public class LoggerService : ILoggerService
{
	private readonly ILogger<LoggerService> _logger;

	public LoggerVerbosity LoggerVerbosity { get; private set; } = LoggerVerbosity.Normal;

	public LoggerService(ILogger<LoggerService> logger) => _logger = logger;

	public void Debug(string message) => Log(message, LogType.Debug);

	public void Success(string message) => Log(message, LogType.Success);

	public void Info(string message) => Log(message, LogType.Important);

	public void Warning(string message) => Log(message, LogType.Warning);

	public void Error(string message) => Log(message, LogType.Error);

	public void Error(Exception exception)
	{
		if (LoggerVerbosity == LoggerVerbosity.Quiet)
		{
			return;
		}

		string message = exception.Message;

		if (LoggerVerbosity >= LoggerVerbosity.Detailed)
		{
			message = $"Message: {exception.Message}\nSource: {exception.Source}\nStackTrace: {exception.StackTrace}";
		}

		Log(message, LogType.Error);
	}

	private void Log(string message, LogType type)
	{
		if (LoggerVerbosity == LoggerVerbosity.Quiet)
		{
			return;
		}

		switch (type)
		{
			case LogType.Debug:
			case LogType.Normal:
				_logger.LogDebug(message);
				break;
			case LogType.Success:
				_logger.LogInformation(message);
				break;
			case LogType.Important:
				_logger.LogWarning(message);
				break;
			case LogType.Warning:
				_logger.LogError(message);
				break;
			case LogType.Error:
				_logger.LogCritical(message);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(type), type, null);
		}
	}

	public void SetLoggerVerbosity(LoggerVerbosity verbosity) => LoggerVerbosity = verbosity;
}
