using Microsoft.Build.Framework;

namespace OllamaClient.Services;
public interface ILoggerService
{
	public void Info(string message);
	public void Info(int messageCode, string message) => Info(message);

	public void Warning(string message);
	public void Warning(int messageCode, string message) => Warning(message);

	public void Error(string message);
	public void Error(int messageCode, string message) => Error(message);

	public void Success(string message);
	public void Success(int messageCode, string message) => Success(message);

	public void Debug(string message);
	public void Debug(int messageCode, string message) => Debug(message);

	public void Error(Exception exception);
	public void Error(int messageCode, Exception exception) => Error(exception);

	public LoggerVerbosity LoggerVerbosity { get; }

	public void SetLoggerVerbosity(LoggerVerbosity verbosity);

}
