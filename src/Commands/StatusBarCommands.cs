using OllamaClient.Services;
using ReactiveUI;

namespace OllamaClient.Commons;
public class StatusBarCommands : ReactiveObject
{
	private readonly IEndpointService<EndpointStatus> _endpointService;
	public StatusBarCommands(IEndpointService<EndpointStatus> endpointService)
	{
		_endpointService = endpointService;
	}

	public void TriggerProgressBar()
	{
		_endpointService.Report(EndpointStatus.Ready);
	}

	public void SetStatusReady()
	{
		_endpointService.Report(EndpointStatus.Ready);
	}

	public void StatusUnavailable() => _endpointService.Report(EndpointStatus.Unavailable);
	
	public void StatusError() => _endpointService.Report(EndpointStatus.Error);

	public void StatusBusy() => _endpointService.Report(EndpointStatus.Busy);

}
