using System.Collections.ObjectModel;
using System.Reactive;
using System.Text.Json;
using System.Windows;
using OllamaClient.Commons;
using OllamaClient.Services;


using OllamaSharp;
using OllamaSharp.Models;

using ReactiveUI;
using Serilog;


namespace OllamaClient.ViewModels;

public class MainViewModel : ReactiveObject, IMainViewModel
{

	#region Properties

	private string title;
	public string Title
	{
		get => title;
		set => this.RaiseAndSetIfChanged(ref title, value);
	}

	private string selectedModel;
	public string SelectedModel
	{
		get => selectedModel;
		set => this.RaiseAndSetIfChanged(ref selectedModel, value);
	}

	private string apiResponse;
	public string ApiResponse
	{
		get => apiResponse;
		set => this.RaiseAndSetIfChanged(ref apiResponse, value);
	}

	private string prompt;
	public string Prompt
	{
		get => prompt;
		set => this.RaiseAndSetIfChanged(ref prompt, value);
	}

	private ObservableCollection<string> models;
	public ObservableCollection<string> Models
	{
		get => models;
		set => this.RaiseAndSetIfChanged(ref models, value);
	}

	private ShowModelResponse _modelResponse;
	public ShowModelResponse ModelResponse
	{
		get => _modelResponse;
		set => this.RaiseAndSetIfChanged(ref _modelResponse, value);
	}

	private bool isLoading;
	public bool IsLoading
	{
		get => isLoading;
		set => this.RaiseAndSetIfChanged(ref isLoading, value);
	}

	private bool isModelsLoaded = false;
	public bool IsModelsLoaded
	{
		get => isModelsLoaded;
		set => this.RaiseAndSetIfChanged(ref isModelsLoaded, value);
	}
	#endregion

	#region Dependencies/Services

	private readonly IOllamaApiClient _apiClient;
	private readonly IModalService _modalService;
	public ReactiveCommand<Unit, Unit> GetModelsCommand { get; }
	public ReactiveCommand<Unit, Unit> ShowModelInfoCommand { get; }
	public ReactiveCommand<Unit, Unit> SendPromptCommand { get; }
	public ReactiveCommand<Unit, Unit> TriggerProgressBarCommand { get; }
	public ReactiveCommand<Unit, Unit> DisconnectEndpointTestCommand { get; }
	public ReactiveCommand<Unit, Unit> TriggerStatusBarCommand { get; }
	public MainViewModel(IOllamaApiClient apiClient, IModalService modalService, StatusBarCommands barCommands)
	{
		_apiClient = apiClient;
		_modalService = modalService;

		Title = "Ollama Client";

		Models = new ObservableCollection<string>();

		Task.Run(async () => await GetModels()).ConfigureAwait(false);

		GetModelsCommand = ReactiveCommand.CreateFromTask(GetModels);
		ShowModelInfoCommand = ReactiveCommand.CreateFromTask(ShowModelInfo);
		SendPromptCommand = ReactiveCommand.CreateFromTask(SendInteractiveChat);

		TriggerProgressBarCommand = ReactiveCommand.Create(barCommands.SetStatusReady);

		DisconnectEndpointTestCommand = ReactiveCommand.Create(barCommands.DisconnectEndpoint);

		TriggerStatusBarCommand = ReactiveCommand.Create(barCommands.SetStatusReady);
	}

	#endregion
	#region Private Methods

	private async Task GetModels()
	{
		try
		{
			var modelList = await _apiClient.ListLocalModels();

			Application.Current.Dispatcher.Invoke(() =>
			{
				Models.Clear();
				foreach (var model in modelList)
				{
					Models.Add(model.Name);
				}
			});

		}
		catch (Exception ex)
		{
			ApiResponse = $"Error fetching models: {ex.Message}";
		}

	}

	public async Task SendInteractiveChat()
	{
		IsLoading = true;

		try
		{
			await Task.Run(async () =>
			{
				Chat? chat = null;
				var ollama = _apiClient;

				ollama.SelectedModel = SelectedModel;
				ApiResponse = string.Empty;


				chat = ollama.Chat(stream =>
				{
					if (stream != null)
					{
						Application.Current.Dispatcher.Invoke(() =>
						{
							IsLoading = false;
							ApiResponse += stream.Message?.Content;
						});
					}
				});

				if (!string.IsNullOrEmpty(Prompt))
				{
					await chat.Send(Prompt);
					Prompt = string.Empty;
				}
			});
		}
		catch (Exception ex)
		{
			Log.Error($"There was an error processing your prompt: {ex.Message}");
			Application.Current.Dispatcher.Invoke(() =>
			{
				MessageBox.Show($"There was an error processing your prompt: {ex.Message}", "Processing Error Prompt", MessageBoxButton.OK, MessageBoxImage.Error);
			});
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

			_modalService.ShowModelInfoModel("Model Information", ApiResponse);
		}
		catch (Exception ex)
		{
			ApiResponse = $"Error fetching model info: {ex.Message}";
		}
	}

	#endregion
}
