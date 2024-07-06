using System.Reactive;

using OllamaClient.Modals;
using OllamaClient.Views;
using ReactiveUI;

namespace OllamaClient.ViewModels;
public class MainViewModel : ReactiveObject
{
	public ReactiveCommand<Unit, Unit> ShowModalCommand { get; }
	public MainViewModel()
	{
		ShowModalCommand = ReactiveCommand.Create(ShowModal);
	}

	private void ShowModal()
	{
		var modal = new Modal
		{
			Title = "Connect To Model",
			Message = "Enter Uri",
		};

		var modalViewModel = new ModalViewModel { Modal = modal };
		var modalView = new ModalView { ViewModel = modalViewModel };

		modalView.ShowDialog();
	}
}
