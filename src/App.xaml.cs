using System.Diagnostics;
using System.IO;
using System.Windows;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OllamaClient.Services.Interfaces;
using OllamaClient.ViewModels;
using OllamaSharp;

using ReactiveUI;

using Splat;

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

		MainWindow mainWindow = host.Services.GetRequiredService<MainWindow>();
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

	private static void LogUnhandledException(Exception exception, string source) {
		Debug.WriteLine("Unhandled exception ({0})", new object[] { source });
		Debug.WriteLine("Exception: {0}", exception);
	}

}