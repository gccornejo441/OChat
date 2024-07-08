using System.Collections.ObjectModel;
using System.Reactive;

using ReactiveUI;

namespace OllamaClient.ViewModels;
public interface IMainViewModel
{
	ReactiveCommand<Unit,Unit> ShowModalCommand { get; }
	ReactiveCommand<Unit, Unit> GetModelsCommand { get; }
	ReactiveCommand<Unit, Unit> ShowModelInfoCommand { get; }
	public string SelectedModel { get; set; }
	public ObservableCollection<string> Models { get; set; }
	public string ApiResponse { get; set; }
	public ObservableCollection<string> SampleList { get; set; }
}
