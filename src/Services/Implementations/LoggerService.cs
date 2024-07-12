using Microsoft.Build.Framework;
using OllamaClient.Services.Interfaces;
using Serilog;

namespace OllamaClient.Services.Implementations;
public class LoggerService : ILoggerService
{
	public LoggerVerbosity LoggerVerbosity { get; private set; } = LoggerVerbosity.Normal;

	public void SetLoggerVerbosity(LoggerVerbosity verbosity) => LoggerVerbosity = verbosity;
	
	public void Debug(string msg)
	{
		Log.Information(msg);
	}

	public void Error(string msg)
	{
		Log.Error(msg);
	}

	public void Error(Exception exception)
	{
		Log.Error(exception, GetExceptionMessage(exception));
	}

	public void Info(string s)
	{
		Log.Information(s);
	}
	public void Info(int id, string s)
	{
		Log.ForContext("InfoCode", id).Information(s);
	}

	public void Success(string msg)
	{
		Log.Information("SUCCESS: {Message}", msg);
	}

	public void Warning(string s)
	{
		Log.Warning(s);
	}

	public string GetExceptionMessage(Exception exception)
	{
		return $"Message: {exception.Message}\nSource: {exception.Source}\nStackTrace: {exception.StackTrace}";
	}
}
