using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OllamaClient.Services.Implementations;
using OllamaClient.Services.Interfaces;
using Serilog;

namespace OllamaClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	private IHost _host;
	public App()
	{
		Directory.SetCurrentDirectory(AppContext.BaseDirectory); // Make sure we're in the right directory

		SetupExceptionHandling();

		Init();
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		var mainWindow = _host.Services.GetRequiredService<MainView>();
		mainWindow.Show();

		try
		{
			var configuration = _host.Services.GetService<IConfiguration>();
			var loggerService = _host.Services.GetService<ILoggerService>();

			
			var appVersion = configuration.GetValue<string>("OllamaClientAppSettings:Version");
			if (string.IsNullOrEmpty(appVersion))
			{
				throw new Exception("App version not found in configuration.");
			}

			loggerService.Info("Starting application");
			loggerService.Info($"Version: {appVersion}");

		}
		catch (Exception ex)
		{

		}
	}

	private void Init()
	{
		_host = GenericHost.CreateHostBuilder().Build();
		_host.Start();

		Log.Logger = new LoggerConfiguration()
			.WriteTo.Debug()
			.CreateLogger();

		Log.Information("WELCOME TO OLLAMA CLIENT!!!!!!!!!!!!!!!!!");

		var settingService = _host.Services.GetRequiredService<ISettingsService>();

	}
	private void SetupExceptionHandling()
	{
		AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
		{
			HandleException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

			// This event is raised when an exception is not caught by
			// any catch block in the application. The IsTerminating
			// property is relevant here to check if the application
			// is going to terminate due to this unhandled exception.

			if (e.IsTerminating)
			{
				Log.CloseAndFlush();
			}
		};

		DispatcherUnhandledException += (sender, e) =>
		{
			HandleException(e.Exception, "Application.DispatcherUnhandledException");
			e.Handled = true;
		};
	}

	protected override void OnExit(ExitEventArgs e)
	{
		_host.Dispose();
		Log.Information("Application termination...");
		Log.CloseAndFlush();

		base.OnExit(e);
	}

	private void HandleException(Exception ex, string source)
	{
		var logger = _host.Services.GetRequiredService<ILoggerService>();
		logger.Error(ex);
		MessageBox.Show($"{source}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
	}

}