using System.Diagnostics;
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
public partial class App
{
	private IHost host;

	public App()
	{
		Init();
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

}