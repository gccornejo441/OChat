using System.Windows.Media;
using OllamaClient.Services;
using ReactiveUI;

namespace OllamaClient.ViewModels
{
	public class StatusBarViewModel : ReactiveObject
	{
		private readonly ISettingsService _settingsService;
		private readonly IEndpointService<EndpointStatus> _endpointService;
		private EndpointStatus _endpointStatus;

		public StatusBarViewModel(IEndpointService<EndpointStatus> endpointService, ISettingsService settingsService)
		{
			_settingsService = settingsService;
			_endpointService = endpointService;
			_currentProject = "O Chat";

			this.WhenAnyValue(x => x._endpointService.Status)
				.Subscribe(x => UpdateStatusAndColor(x));
		}

		private void UpdateStatusAndColor(EndpointStatus status)
		{
			EndpointStatus = status;
			Progress = status == EndpointStatus.Ready ? 100 : 50;
			BarColor = status switch
			{
				EndpointStatus.Unavailable => new BrushConverter().ConvertFromString("#951C2D") as SolidColorBrush ?? Brushes.Black,
				EndpointStatus.Ready => new BrushConverter().ConvertFromString("#275C4C") as SolidColorBrush ?? Brushes.Black,
				_ => BarColor
			};
		}

		#region Properties
		public EndpointStatus EndpointStatus
		{
			get => _endpointStatus;
			set => this.RaiseAndSetIfChanged(ref _endpointStatus, value);
		}

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
