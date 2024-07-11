
using OllamaClient.Services.Interfaces;

namespace OllamaClient.Services.Implementations;
public class SettingsService : ISettingsService
{
	private bool isLoaded;
	public const string ProductName = "OllamaClient";
	public const string AssemblyName = ProductName + ".dll";

	public SettingsService()
	{
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
}
