using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Text.Json;
using System.Xml;

using OllamaClient.Modals;
using OllamaClient.Views;

using OllamaSharp;
using OllamaSharp.Models;

using ReactiveUI;

using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace OllamaClient.ViewModels;
public class MainViewModel : ReactiveObject
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

	private ShowModelResponse _modelResponse;
	public ShowModelResponse ModelResponse
	{
		get => _modelResponse;
		set => this.RaiseAndSetIfChanged(ref _modelResponse,value);
	}

	private ObservableCollection<string> sampleList = new ObservableCollection<string>
	{
			"Turkey",
			"Ostrich"
	};

	public ObservableCollection<string> SampleList
	{
		get => sampleList;
		set => this.RaiseAndSetIfChanged(ref sampleList,value);
	}

	public MainViewModel(IOllamaApiClient apiClient)
	{
		_apiClient = apiClient;

		SampleList = new ObservableCollection<string>();

		Models = new ObservableCollection<string>();
		ShowModalCommand = ReactiveCommand.Create(ShowModal);
		GetModelsCommand = ReactiveCommand.CreateFromTask(GetModels);
		ShowModelInfoCommand = ReactiveCommand.CreateFromTask(ShowModelInfo);

		InitializationEvents();
	}

    private void InitializationEvents()
    {
        
    }

    private async void ShowModal()
	{
		var modal = new Modal("Connect To Model","Enter Uri");

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
			if (string.IsNullOrEmpty(SelectedModel))
			{
				ApiResponse = "No model selected.";
				return;
			}


			var info = await _apiClient.ShowModelInformation(SelectedModel);
			ModelResponse = info;

			ApiResponse = JsonSerializer.Serialize(info, new JsonSerializerOptions { WriteIndented = true });

			var modal = new Modal("Model Information", ApiResponse);
			var modalViewModel = new ModalViewModel { Modal = modal };
			var modalView = new ModalView { ViewModel = modalViewModel };

			modalView.ShowDialog();

		}
		catch (Exception ex)
		{
			ApiResponse = $"Error fetching model info: {ex.Message}";
		}
	}

}
