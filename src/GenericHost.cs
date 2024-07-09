using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using OllamaClient.ViewModels;

using OllamaSharp;

using ReactiveUI;

using Wpf.Ui;

namespace OllamaClient;
public static class GenericHost
{
	public static IHostBuilder CreateHostBuilder() => Host
		.CreateDefaultBuilder()
		.ConfigureAppConfiguration((context,config) =>
		{
			config.AddJsonFile("appsettings.json",optional: true);
		})
		.ConfigureServices((context,services) =>
		{
			services.AddSingleton<IModalViewModel,ModalViewModel>();
			services.AddSingleton<MainViewModel>();
			services.AddSingleton<MainWindow>();
			services.AddHttpClient<IOllamaApiClient,OllamaApiClient>(client =>
			{
				client.BaseAddress = new Uri("http://localhost:11434");
			});
		})
		.ConfigureLogging(logging =>
		{
			logging.ClearProviders();
			logging.AddConsole();
		})
		.UseEnvironment(Environments.Development);



}
