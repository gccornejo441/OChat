using System.IO;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OllamaClient.Services.Interfaces;

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

		SetupExceptionHandling();
		var settingService = host.Services.GetRequiredService<ISettingsService>();
		//if (settingService.IsHealthy()) throw new FileNotFoundException("File not found");
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		MainView mainWindow = host.Services.GetRequiredService<MainView>();
		mainWindow.Show();
	}

	private void SetupExceptionHandling()
	{
		AppDomain.CurrentDomain.UnhandledException += (sender,e) =>
		{
			MessageBox.Show(e.ExceptionObject.ToString(),"Error",MessageBoxButton.OK,MessageBoxImage.Error);
		};

		this.DispatcherUnhandledException += (sender,e) =>
		{

		};
	}

	private void Init()
	{
		host = GenericHost.CreateHostBuilder().Build();
		host.Start();
	}

	protected override void OnExit(ExitEventArgs e)
	{
		host.Dispose();
		base.OnExit(e);
	}


}