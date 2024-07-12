using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OllamaClient.Services.Implementations;
using OllamaClient.Services.Interfaces;
using OllamaClient.ViewModels;
using OllamaSharp;
using Serilog;

namespace OllamaClient;
public static class GenericHost
{
	public static IHostBuilder CreateHostBuilder() => Host
		.CreateDefaultBuilder()
		.ConfigureAppConfiguration((context, config) =>
		{
			config.SetBasePath(Path.GetDirectoryName(System.AppContext.BaseDirectory))
				  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
		})
		.ConfigureServices((context, services) =>
		{
			services.AddHostedService<AppBackgroundService>();
			services.AddSingleton<IModalViewModel, ModalViewModel>();
			services.AddSingleton<ISettingsService, SettingsService>();
			services.AddSingleton<ILoggerService, LoggerService>();
			services.AddSingleton<MainViewModel>();
			services.AddSingleton<MainView>();
			services.AddSingleton<IModalService, ModalService>();
			services.AddHttpClient<IOllamaApiClient, OllamaApiClient>(client =>
			{
				client.BaseAddress = new Uri("http://localhost:11434");
			});
		})
		.ConfigureLogging((context, logging) =>
		{
			logging.ClearProviders();
			logging.AddDebug();
			logging.AddSerilog(new LoggerConfiguration()
				.ReadFrom.Configuration(context.Configuration)
				.WriteTo.Debug()
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