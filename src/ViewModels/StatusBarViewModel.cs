using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using OllamaClient.Services;
using ReactiveUI;

namespace OllamaClient.ViewModels
{
	public class StatusBarViewModel : ReactiveObject
	{
		private readonly ISettingsService _settingsService;
		private readonly IProgressService<double> _progressService;
		private readonly IEndpointService<EndpointStatus> _endpointService;
		private EndpointStatus _endpointStatus;

		public StatusBarViewModel(
			ISettingsService settingsService,
			IProgressService<double> progressService,
			IEndpointService<EndpointStatus> endpointService)
		{
			_settingsService = settingsService;
			_endpointService = endpointService;

			_currentProject = "O Chat";
			_endpointService.StatusChanged += OnEndpointStatusChanged;
		
			//_progressService = progressService;
			//_progressService.ProgressChanged += OnProgressChanged;
			//_progressService.PropertyChanged += OnProgressPropertyChanged;
		}


		public EndpointStatus EndpointStatus
		{
			get => _endpointStatus;
			set => this.RaiseAndSetIfChanged(ref _endpointStatus, value);
		}

		private void OnEndpointStatusChanged(object? sender, EndpointStatus e)
		{
			IsIndeterminate = false;
			EndpointStatus = e;
			double progressValue = e == EndpointStatus.Available ? 100 : 50;
			Progress = progressValue;
		}

		//private void OnProgressChanged(object? sender, double e)
		//{
		//	Progress = e * 100;
		//}

		//private void OnProgressPropertyChanged(object? sender, PropertyChangedEventArgs e)
		//{
		//	Application.Current.Dispatcher.BeginInvoke(() =>
		//	{
		//		switch (e.PropertyName)
		//		{
		//			case nameof(IProgressService<double>.IsIndeterminate):
		//				IsIndeterminate = _progressService.IsIndeterminate;
		//				break;

		//			case nameof(IProgressService<double>.Status):
		//				UpdateStatusAndColor();
		//				break;
		//		}
		//	});
		//}


		//private void UpdateStatusAndColor()
		//{
		//	Status = _progressService.Status.ToString();
		//	BarColor = _progressService.Status switch
		//	{
		//		AppStatus.Running => Brushes.DarkOrange,
		//		AppStatus.Ready => new BrushConverter().ConvertFromString("#951C2D") as SolidColorBrush ?? Brushes.Black,
		//		_ => BarColor
		//	};
		//}

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

		public string VersionNumber => _settingsService.GetVersionNumber();

		private Brush _barColor = Brushes.Black;
		public Brush BarColor
		{
			get => _barColor;
			set => this.RaiseAndSetIfChanged(ref _barColor, value);
		}

		#endregion
	}
}
