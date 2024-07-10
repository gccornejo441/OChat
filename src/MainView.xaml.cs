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
public partial class MainView : Window, IViewFor<MainViewModel>
{
	public static readonly DependencyProperty ViewModelProperty =
	DependencyProperty.Register(
		"ViewModel",
		typeof(MainViewModel),
		typeof(MainView));

	public MainView(MainViewModel mainViewModel)
	{
		InitializeComponent();

		ViewModel = mainViewModel;
		DataContext = ViewModel;
		MainWindowInitialization();

		this.WhenActivated(disposables =>
		{
			this.OneWayBind(ViewModel,
				vm => vm.SampleList,
				v => v.sampleListCb.ItemsSource).DisposeWith(disposables);
	});

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
