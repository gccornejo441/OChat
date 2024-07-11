using System.Collections.ObjectModel;
using System.Reactive;
using System.Text.Json;

using OllamaClient.Services.Interfaces;

using OllamaSharp;
using OllamaSharp.Models;

using ReactiveUI;

namespace OllamaClient.ViewModels;
public class MainViewModel : ReactiveObject
{
    private readonly IOllamaApiClient _apiClient;
    private readonly IModalService _modalService;

    public ReactiveCommand<Unit, Unit> ShowModalCommand { get; }
    public ReactiveCommand<Unit, Unit> GetModelsCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowModelInfoCommand { get; }
    public ReactiveCommand<Unit, Unit> SendPromptCommand { get; }

    private string selectedModel;
    public string SelectedModel
    {
        get => selectedModel;
        set => this.RaiseAndSetIfChanged(ref selectedModel, value);
    }

    private ObservableCollection<string> models;
    public ObservableCollection<string> Models
    {
        get => models;
        set => this.RaiseAndSetIfChanged(ref models, value);
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

    private ShowModelResponse _modelResponse;
    public ShowModelResponse ModelResponse
    {
        get => _modelResponse;
        set => this.RaiseAndSetIfChanged(ref _modelResponse, value);
    }

    public MainViewModel(IOllamaApiClient apiClient, IModalService modalService)
    {
        _apiClient = apiClient;
        _modalService = modalService;

        Models = new ObservableCollection<string>();
        ShowModalCommand = ReactiveCommand.Create(ShowModal);
        GetModelsCommand = ReactiveCommand.CreateFromTask(GetModels);
        ShowModelInfoCommand = ReactiveCommand.CreateFromTask(ShowModelInfo);
        SendPromptCommand = ReactiveCommand.CreateFromTask(SendInteractiveChat);
    }

    private async void ShowModal()
    {
        _modalService.ShowModelInfoModel("API Response", "Enter URI");
    }

    private async Task GetModels()
    {
        try
        {
            IEnumerable<Model> modelList = await _apiClient.ListLocalModels();
            Models.Clear();
            foreach (Model model in modelList)
            {
                Models.Add(model.Name);
            }
        }
        catch (Exception ex)
        {
            ApiResponse = $"Error fetching models: {ex.Message}";
        }
    }

    public async Task SendInteractiveChat()
    {
        var ollama = _apiClient;
        ollama.SelectedModel = SelectedModel;

        var chat = ollama.Chat(stream =>
        {
            if (stream != null)
            {
                ApiResponse += stream.Message?.Content;
            }
        });

        if (!string.IsNullOrEmpty(Prompt))
        {
            await chat.Send(Prompt);
            Prompt = string.Empty;
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

            ShowModelResponse info = await _apiClient.ShowModelInformation(SelectedModel);
            ModelResponse = info;

            ApiResponse = JsonSerializer.Serialize(info, new JsonSerializerOptions { WriteIndented = true });

            _modalService.ShowModelInfoModel("Model Information", ApiResponse);
        }
        catch (Exception ex)
        {
            ApiResponse = $"Error fetching model info: {ex.Message}";
        }
    }

}
