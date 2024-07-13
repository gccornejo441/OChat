using System.Windows;

using OllamaClient.ViewModels;

namespace OllamaClient;

/// <summary>
/// Interaction logic for MainView.xaml
/// </summary>
public partial class MainView : Wpf.Ui.Controls.FluentWindow
{
	public MainView(IMainViewModel mainViewModel)
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
