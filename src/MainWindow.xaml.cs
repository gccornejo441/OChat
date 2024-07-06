using System.Reactive.Disposables;
using System.Windows;

using OllamaClient.ViewModels;

using ReactiveUI;

namespace OllamaClient;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IViewFor<MainViewModel>
{
	public static readonly DependencyProperty ViewModalProperty = DependencyProperty.Register(
	"ViewModel",typeof(MainViewModel),typeof(MainWindow),new PropertyMetadata(null));

	public MainWindow()
	{
		InitializeComponent();

		this.WhenActivated(d =>
		{
			this.BindCommand(
				ViewModel,
				vm => vm.ShowModalCommand,
				v => v.ShowDialogButton).DisposeWith(d);

		});

		ViewModel = new MainViewModel();

	}

	public MainViewModel? ViewModel
	{
		get => (MainViewModel)GetValue(ViewModalProperty);

		set => SetValue(ViewModalProperty,value);
	}

	object? IViewFor.ViewModel
	{
		get => ViewModel;

		set => ViewModel = (MainViewModel)value;
	}
}