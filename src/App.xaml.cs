using System.Windows;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
		host = Host.CreateDefaultBuilder()
			.ConfigureAppConfiguration((context,config) =>
			{
				config.AddJsonFile("appsettings.json",optional: true);
			})
			.ConfigureServices((context,services) =>
			{
				ConfigureServices(services);
			})
			.ConfigureLogging(logging =>
			{
				logging.ClearProviders();
				logging.AddConsole();
			})
			.Build();
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		MainWindow mainWindow = host.Services.GetRequiredService<MainWindow>();
		mainWindow.Show();

	}

	private void ConfigureServices(IServiceCollection services)
	{
		services.AddSingleton<IModalViewModel,ModalViewModel>();
		services.AddSingleton<IMainViewModel,MainViewModel>();
		services.AddSingleton<MainWindow>();
		services.AddHttpClient<IOllamaApiClient,OllamaApiClient>(client =>
		{
			client.BaseAddress = new Uri("http://localhost:11434");
		});
	}

	protected override void OnExit(ExitEventArgs e)
	{
		host.Dispose();
		base.OnExit(e);
	}

}