using OllamaClient.Modals;
using OllamaClient.ViewModels;
using OllamaClient.Views;

namespace OllamaClient.Services;
public class ModalService : IModalService
{
    public void ShowModelInfoModel(string title, string message)
    {
        var modal = new Modal(title, message);
        var modalViewModel = new ModalViewModel { Modal = modal };
        var modalView = new ModalView { ViewModel = modalViewModel };

        modalView.ShowDialog();
    }
}
