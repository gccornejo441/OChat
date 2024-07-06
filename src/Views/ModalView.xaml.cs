using System.Reactive.Disposables;
using System.Windows;

using OllamaClient.ViewModels;

using ReactiveUI;

namespace OllamaClient.Views;
/// <summary>
/// Interaction logic for ModalView.xaml
/// </summary>
public partial class ModalView : Window, IViewFor<ModalViewModel>
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel",typeof(ModalViewModel),typeof(ModalView),new PropertyMetadata(null));

	public ModalView()
	{
		InitializeComponent();

		this.WhenActivated(disposables =>
		{
			this.Bind(ViewModel,
				vm => vm.Modal.Title,
				v => v.SuperModal.Title).DisposeWith(disposables);
			this.Bind(ViewModel,
				vm => vm.Modal.Message,
				v => v.ModalMessage.Text).DisposeWith(disposables);

			this.BindCommand(ViewModel,
				vm => vm.CloseCommand,
				v => v.CloseButton).DisposeWith(disposables);
		});

		ViewModel = new ModalViewModel();
	}

	public ModalViewModel? ViewModel
	{
		get => (ModalViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty,value);
	}

	object? IViewFor.ViewModel
	{
		get => ViewModel;
		set => ViewModel = (ModalViewModel)value;
	}
}
