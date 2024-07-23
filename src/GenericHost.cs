using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OllamaClient.Commons;
using OllamaClient.Services;
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
			// Register configuration
			services.AddSingleton<IConfiguration>(context.Configuration);

			// Add hosted service
			services.AddHostedService<AppBackgroundService>();

			// Add view models and commands
			services.AddSingleton<StatusBarViewModel>();
			services.AddSingleton<StatusBarCommands>();

			// Register the URL configuration
			string apiBaseUrl = context.Configuration.GetValue<string>("JSONPlaceholderSetting:ApiBaseUrl");
			services.AddSingleton(new EndpointService(apiBaseUrl));

			// Add services with implementations
			services.AddSingleton<IEndpointService<EndpointStatus>>(provider => provider.GetRequiredService<EndpointService>());
			services.AddSingleton<IProgressService<double>, ProgressService<double>>();
			services.AddSingleton<IModalViewModel, ModalViewModel>();
			services.AddSingleton<ISettingsService, SettingsService>();
			services.AddSingleton<ILoggerService, LoggerService>();
			services.AddSingleton<IMainViewModel, MainViewModel>();
			services.AddSingleton<MainView>();
			services.AddSingleton<IModalService, ModalService>();

			// Configure HttpClient
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