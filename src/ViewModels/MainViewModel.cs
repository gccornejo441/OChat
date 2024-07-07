using System.Collections.ObjectModel;
using System.Reactive;
using System.Text.Json;
using System.Xml;

using OllamaClient.Modals;
using OllamaClient.Views;

using OllamaSharp;

using ReactiveUI;

namespace OllamaClient.ViewModels;
public class MainViewModel : ReactiveObject, IMainViewModel
{
	private readonly IOllamaApiClient _apiClient;

	public ReactiveCommand<Unit,Unit> ShowModalCommand { get; }

	public ReactiveCommand<Unit,Unit> GetModelsCommand { get; }

	public ReactiveCommand<Unit,Unit> ShowModelInfoCommand { get; }

	private string selectedModel;
	public string SelectedModel
	{
		get => selectedModel;
		set => this.RaiseAndSetIfChanged(ref selectedModel,value);
	}

	private ObservableCollection<string> models;
	public ObservableCollection<string> Models
	{
		get => models;
		set => this.RaiseAndSetIfChanged(ref models,value);
	}

	private string apiResponse;
	public string ApiResponse
	{
		get => apiResponse;
		set => this.RaiseAndSetIfChanged(ref apiResponse,value);
	}

	public MainViewModel(IOllamaApiClient apiClient)
	{
		_apiClient = apiClient;

		Models = new ObservableCollection<string>();

		ShowModalCommand = ReactiveCommand.Create(ShowModal);
		GetModelsCommand = ReactiveCommand.CreateFromTask(GetModels);
		ShowModelInfoCommand = ReactiveCommand.CreateFromTask(ShowModelInfo);
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

	private async Task GetModels()
	{
		try
		{
			var models = await _apiClient.ListLocalModels();
			Models.Clear();
			foreach (var model in models)
			{
				Models.Add(model.Name);
			}
		}
		catch (Exception ex)
		{
			ApiResponse = $"Error fetching models: {ex.Message}";
		}
	}

	private async Task ShowModelInfo()
	{
		try
		{
			var info = await _apiClient.ShowModelInformation(SelectedModel);
			ApiResponse = JsonSerializer.Serialize(info,new JsonSerializerOptions { WriteIndented = true });
		}
		catch (Exception ex)
		{
			ApiResponse = $"Error fetching model info: {ex.Message}";
		}
	}

}
