using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using OllamaClient.ViewModels;

using OllamaSharp;

using ReactiveUI;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

using Splat;

using Wpf.Ui;

namespace OllamaClient;
public static class GenericHost
{
	public static IHostBuilder CreateHostBuilder() => Host
		.CreateDefaultBuilder()
		.ConfigureAppConfiguration((context, config) =>
		{
			config.SetBasePath(Path.GetDirectoryName(System.AppContext.BaseDirectory));
			config.AddJsonFile("appsettings.json", optional: true);
		})
		.ConfigureServices((context, services) =>
		{
			services.AddHostedService<AppBackgroundService>();
			services.AddSingleton<IModalViewModel, ModalViewModel>();
			services.AddSingleton<MainViewModel>();
			services.AddSingleton<MainView>();
			services.AddHttpClient<IOllamaApiClient, OllamaApiClient>(client =>
			{
				client.BaseAddress = new Uri("http://localhost:11434");
			});

			Locator.CurrentMutable.Register(() => new MainView(Locator.Current.GetService<MainViewModel>()),typeof(IViewFor<MainViewModel>));
		})
		.ConfigureLogging((context, logging) =>
		{
			logging.ClearProviders();
			logging.AddSerilog(new LoggerConfiguration()
				.ReadFrom.Configuration(context.Configuration)
				.WriteTo.Console(theme: AnsiConsoleTheme.Literate)
				.CreateLogger());
		})
		.UseEnvironment(Environments.Development);

}

public class AppBackgroundService : IHostedService
{
	private readonly ILogger<AppBackgroundService> _logger;

	public AppBackgroundService(ILogger<AppBackgroundService> logger)
	{
		_logger = logger;
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("MyBackgroundService is starting.");
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("MyBackgroundService is stopping.");
		return Task.CompletedTask;
	}
}