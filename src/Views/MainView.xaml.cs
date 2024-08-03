using System.Windows;
using OllamaClient.Services;
using OllamaClient.ViewModels;

namespace OllamaClient;

/// <summary>
/// Interaction logic for MainView.xaml
/// </summary>
public partial class MainView : Wpf.Ui.Controls.FluentWindow
{
	private SystemTrayService _systemTrayService;

	public MainView(IMainViewModel mainViewModel, StatusBarViewModel statusBarViewModel, SystemTrayService systemTrayService)
	{
		InitializeComponent();

		_systemTrayService = systemTrayService;

		DataContext = mainViewModel;
		this.statusBarUserControl.DataContext = statusBarViewModel;
		this.StateChanged += systemTrayService.HandleWindowStateChange;

	}


	protected override void OnClosed(EventArgs e)
	{
		_systemTrayService.Dispose();
		base.OnClosed(e);
	}

}
