using System.Reactive;
using OllamaClient.Modals;
using ReactiveUI;

namespace OllamaClient.ViewModels;

public interface IModalViewModel
{
    Modal Modal { get; set; }
}
public class ModalViewModel : ReactiveObject, IModalViewModel
{
    private Modal modal;
    public Modal Modal
    {
        get => modal;

        set => this.RaiseAndSetIfChanged(ref modal, value);
    }

    public ReactiveCommand<Unit, Unit> CloseCommand { get; }
    public ModalViewModel()
    {
        CloseCommand = ReactiveCommand.Create(CloseModal);
    }

    private void CloseModal()
    {

    }
}
