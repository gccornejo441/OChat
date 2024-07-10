using System.IO;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OllamaClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	private IHost host;
	public App()
	{
		Directory.SetCurrentDirectory(AppContext.BaseDirectory); // Make sure we're in the right directory

		Init();

		// var settingsService = Locator.Current.GetService<ISettingsService>();
		// if (settingsService.IsHealthy()) throw new FileNotFoundException("File not found");
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		MainView mainWindow = host.Services.GetRequiredService<MainView>();
		mainWindow.Show();
	}

	protected override void OnExit(ExitEventArgs e)
	{
		host.Dispose();
		base.OnExit(e);
	}

	private void Init()
	{
		host = GenericHost.CreateHostBuilder().Build();
	}

}