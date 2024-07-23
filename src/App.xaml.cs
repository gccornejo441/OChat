using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OllamaClient.Services;
using Serilog;

namespace OllamaClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	private IHost? _host = null;
	public App()
	{
		Directory.SetCurrentDirectory(AppContext.BaseDirectory);

		SetupExceptionHandling();

		Init();
	}
	private void Init()
	{
		try
		{

			_host = GenericHost.CreateHostBuilder().Build();
			_host.Start();

			Log.Logger = new LoggerConfiguration()
				.WriteTo.Debug()
				.CreateLogger();

			Log.Information("WELCOME TO OLLAMA CLIENT!!!!!!!!!!!!!!!!!");
		}
		catch (HostAbortedException ex)
		{
			Log.Error(ex, "The host start process was aborted.");
			MessageBox.Show("The host is not available. Application cannot start.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			Current.Shutdown();
		}

		// TODO
		//var settingService = _host.Services.GetRequiredService<ISettingsService>();
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);
		ILoggerService? loggerService = null;

		try
		{
			if (_host == null)
			{
				HostFailedMessageBox();
			}

			var configuration = _host.Services.GetService<IConfiguration>();
			loggerService = _host.Services.GetService<ILoggerService>();

			if (loggerService == null || configuration == null)
			{
				MessageBox.Show("Critical services are not available. Application cannot start.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				Current.Shutdown();
				return;
			}
			var appVersion = configuration.GetValue<string>("OllamaClientAppSettings:Version");

			if (string.IsNullOrEmpty(appVersion))
			{
				var edgeCaseMessage = "App version not found in configuration.";
				loggerService.Debug(edgeCaseMessage);
				throw new ArgumentNullException(edgeCaseMessage);
			}

			loggerService.Debug("Starting application");
			loggerService.Debug($"Version: {appVersion}");


		}
		catch (Exception ex)
		{
			loggerService?.Warning(1, $"Startup exception {ex}");
			Log.Warning(ex.ToString());
		}

		var mainWindow = _host?.Services.GetRequiredService<MainView>();
		mainWindow.Show();

	}

	private void SetupExceptionHandling()
	{
		AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
		{

			HandleExceptionError((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

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

			try
			{
				HandleExceptionError(e.Exception, "Application.DispatcherUnhandledException");
				e.Handled = true;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "An error occurred while handling a dispatcher unhandled exception.");
			}
		};
	}

	protected override void OnExit(ExitEventArgs e)
	{
		_host.Dispose();
		Log.Information("Application termination...");
		Log.CloseAndFlush();

		base.OnExit(e);
	}
	private void HostFailedMessageBox()
	{
		MessageBox.Show($"Host is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
	}

	private void HandleExceptionError(Exception ex, string source)
	{
		if (_host is null)
		{
			throw new ArgumentNullException($"Host is not initialized: {_host}");
		}
		else
		{
			var logger = _host.Services.GetRequiredService<ILoggerService>();
			logger.Error(ex);
		}

		Log.CloseAndFlush();
		Current.Shutdown();
		MessageBox.Show($"{source}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
	}

}