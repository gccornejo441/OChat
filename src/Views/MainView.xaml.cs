using System.Windows;

using OllamaClient.ViewModels;

namespace OllamaClient;

/// <summary>
/// Interaction logic for MainView.xaml
/// </summary>
public partial class MainView : Wpf.Ui.Controls.FluentWindow
{
	public MainView(IMainViewModel mainViewModel, StatusBarViewModel statusBarViewModel)
	{
		InitializeComponent();

		DataContext = mainViewModel;
		this.statusBarUserControl.DataContext = statusBarViewModel;

	}
}
