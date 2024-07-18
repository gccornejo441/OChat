
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using OllamaClient.Services.Interfaces;

namespace OllamaClient.Services.Implementations;
public class SettingsService : ISettingsService
{
	private bool isLoaded;
	public const string ProductName = "OllamaClient";
	public const string AssemblyName = ProductName + ".dll";
	private readonly IConfiguration _configuration;
	private readonly ILoggerService _loggerService;

	public SettingsService(IConfiguration configuration, ILoggerService loggerService)
	{
		_configuration = configuration;
		_loggerService = loggerService;
	}

	public bool IsHealthy()
	{
		isLoaded = true;
		return isLoaded;
	}

	public void Save()
	{
		throw new NotImplementedException();
	}

	public string GetVersionNumber()
	{
		try
		{
			var appVersion = _configuration.GetValue<string>("OllamaClientAppSettings:Version");
			if (string.IsNullOrEmpty(appVersion))
			{
				throw new ArgumentNullException(appVersion, "Version number cannot be null or empty.");
			}
			return appVersion;
		}
		catch (Exception ex)
		{
			_loggerService.Warning($"Application setting data is missing: {ex}");
			// TODO: Message box
			return string.Empty;
		}
	}

}
