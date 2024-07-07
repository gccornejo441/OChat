using System.Reactive.Disposables;
using System.Windows;

using OllamaClient.ViewModels;

using ReactiveUI;

namespace OllamaClient;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IViewFor<IMainViewModel>
{
	public static readonly DependencyProperty ViewModalProperty = DependencyProperty.Register(
	"ViewModel",typeof(IMainViewModel),typeof(MainWindow),new PropertyMetadata(null));

	public MainWindow(IMainViewModel IMainViewModel)
	{
		InitializeComponent();

		this.WhenActivated(d =>
		{
			this.BindCommand(
				ViewModel,
				vm => vm.ShowModalCommand,
				v => v.ShowDialogButton).DisposeWith(d);
			this.BindCommand(
				ViewModel,
				vm => vm.ShowModelInfoCommand,
				v => v.ShowModelInfo).DisposeWith(d);
			this.BindCommand(
				ViewModel,
				vm => vm.GetModelsCommand,
				v => v.GetModels).DisposeWith(d);
			this.Bind(
				ViewModel,
				vm => vm.ApiResponse,
				v => v.ApiResponseTB.Text).DisposeWith(d);
			this.OneWayBind(
				ViewModel,
				vm => vm.Models,
				v => v.ModelsCB.ItemsSource).DisposeWith(d); 
		});


		ViewModel = IMainViewModel;

	}

	public IMainViewModel? ViewModel
	{
		get => (IMainViewModel)GetValue(ViewModalProperty);

		set => SetValue(ViewModalProperty,value);
	}

	object? IViewFor.ViewModel
	{
		get => ViewModel;

		set => ViewModel = (IMainViewModel)value;
	}
}