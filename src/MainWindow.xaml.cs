using System.Windows;

using OllamaClient.ViewModels;

namespace OllamaClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow(MainViewModel mainViewModel)
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
}
