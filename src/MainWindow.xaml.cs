using System.Reactive.Disposables;
using System.Windows;

using OllamaClient.ViewModels;

using OllamaSharp;

using ReactiveUI;

using Wpf.Ui;

namespace OllamaClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, IViewFor<MainViewModel>
	{
		public static readonly DependencyProperty ViewModelProperty =
		DependencyProperty.Register(
			"ViewModel",
			typeof(MainViewModel),
			typeof(MainWindow));

		public MainWindow(MainViewModel mainViewModel)
		{
			InitializeComponent();

			this.WhenActivated(disposables =>
			{
				this.OneWayBind(ViewModel,
					v => v.SampleList,
					vm => vm.sampleListCb.ItemsSource).DisposeWith(disposables);
			});

			ViewModel = mainViewModel;
			MainWindowInitialization();
		}


		public MainViewModel? ViewModel
		{
			get => (MainViewModel)GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty,value);
		}

		object? IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (MainViewModel)value;
		}

		private void MainWindowInitialization()
		{
			this.Height = 350;
			this.Width = 525;
			this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
		}

	}
}
