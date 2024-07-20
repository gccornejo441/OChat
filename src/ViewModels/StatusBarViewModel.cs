using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

using OllamaClient.Services.Interfaces;
using ReactiveUI;

namespace OllamaClient.ViewModels;
public class StatusBarViewModel : ReactiveObject
{
	private readonly ISettingsService _settingService;
	private readonly IProgressService<double> _progressService;

	public StatusBarViewModel(ISettingsService settingsService, IProgressService<double> progressService)
	{
		_progressService = progressService;
		_settingService = settingsService;
		_currentProject = "O Chat";

		_progressService.ProgressChanged += OnProgressChanged;
		_progressService.PropertyChanged += OnPropertyChanged;
	}

	private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(IProgressService<double>.IsIndeterminate))
		{
			Application.Current.Dispatcher.BeginInvoke(() =>
			{
				IsIndeterminate = _progressService.IsIndeterminate;
			});
		}
		else if (e.PropertyName == nameof(IProgressService<double>.Status))
		{
			Application.Current.Dispatcher.BeginInvoke(() =>
			{
				Status = _progressService.Status.ToString();
				switch (_progressService.Status)
				{
					case AppStatus.Running:
						BarColor = Brushes.DarkOrange;
						break;
					case AppStatus.Ready:
						if (new BrushConverter().ConvertFromString("#951C2D") is SolidColorBrush brush)
						{
							BarColor = brush;
						}
						break;
					default:
						break;
				}
			});
		}
	}

	private void OnProgressChanged(object? sender, double e)
	{
		Progress = e * 100;
	}

	#region Event Method


	#endregion

	#region Properties
	private double _progress;
	public double Progress
	{
		get => _progress;
		set => this.RaiseAndSetIfChanged(ref _progress, value);
	}

	private bool _isIndeterminate;
	public bool IsIndeterminate
	{
		get => _isIndeterminate;
		set => this.RaiseAndSetIfChanged(ref _isIndeterminate, value);
	}

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
		get => _barColor;
		set => this.RaiseAndSetIfChanged(ref _barColor, value);
	}

	#endregion
}