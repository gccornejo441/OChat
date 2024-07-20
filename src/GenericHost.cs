using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OllamaClient.Services;
using OllamaClient.Services.Implementations;
using OllamaClient.Services.Interfaces;
using OllamaClient.ViewModels;
using OllamaSharp;
using OllamaSharp.Models.Chat;
using OllamaSharp.Streamer;

namespace OllamaClient;
public static class GenericHost
{
	public static IHostBuilder CreateHostBuilder() => Host
		.CreateDefaultBuilder()
		.ConfigureAppConfiguration((context, config) =>
		{
			var basePath = Path.GetDirectoryName(AppContext.BaseDirectory);
			config.SetBasePath(basePath)
				  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
		})
		.ConfigureServices((context, services) =>
		{
			services.AddHostedService<AppBackgroundService>();
			services.AddSingleton<StatusBarViewModel>();
			services.AddSingleton<IProgressService<double>, ProgressService<double>>();
			services.AddSingleton<IModalViewModel, ModalViewModel>();
			services.AddSingleton<ISettingsService, SettingsService>();
			services.AddSingleton<ILoggerService, LoggerService>();
			services.AddSingleton<IMainViewModel, MainViewModel>();
			services.AddSingleton<MainView>();
			services.AddSingleton<IModalService, ModalService>();
			services.AddHttpClient<IOllamaApiClient, OllamaApiClient>(client =>
			{
				client.BaseAddress = new Uri("http://localhost:11434");
			});
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

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		try
		{
			_logger.LogInformation("MyBackgroundService is starting.");
			await Task.CompletedTask;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while starting the background service.");
			throw;
		}

	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("MyBackgroundService is stopping.");
		return Task.CompletedTask;
	}
}