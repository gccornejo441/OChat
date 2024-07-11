using System;
using System.Reactive.Disposables;
using System.Windows;

using OllamaClient.ViewModels;

using ReactiveUI;

using Splat;

namespace OllamaClient;

/// <summary>
/// Interaction logic for MainView.xaml
/// </summary>
public partial class MainView : Window
{
	public MainView(MainViewModel mainViewModel)
	{
		InitializeComponent();

		DataContext = mainViewModel;
		MainWindowInitialization();
	}

	private void MainWindowInitialization()
	{
		this.Height = 350;
		this.Width = 525;
		this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
	}

}
