using System.Windows.Media;

using OllamaClient.Services.Interfaces;
using ReactiveUI;

namespace OllamaClient.ViewModels;
public class StatusBarViewModel : ReactiveObject
{
	private readonly ISettingsService _settingService;

	public StatusBarViewModel(ISettingsService settingsService)
	{
		_settingService = settingsService;
		CurrentProject = "Ollama Client";
	}

	#region Properties
	private string _status = "Ready";
	public string Status
	{
		get => _status;
		set => this.RaiseAndSetIfChanged(ref _status, value);
	}

	private string _currentProject;
	public string CurrentProject
	{
		get => _currentProject;
		set => this.RaiseAndSetIfChanged(ref _currentProject, value);
	}

	public string VersionNumber => _settingService.GetVersionNumber();

	private Brush _barColor = Brushes.Black;

	public Brush BarColor
	{
		get => BarColor;
		set => this.RaiseAndSetIfChanged(ref _barColor, value);
	}

	#endregion
}