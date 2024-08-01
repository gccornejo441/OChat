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
		_endpointService.Report(EndpointStatus.Available);
	}

	public void SetStatusReady()
	{
		_endpointService.Report(EndpointStatus.Available);
	}

	public void DisconnectEndpoint()
	{
		_endpointService.Report(EndpointStatus.Unavailable);
	}

	public void CheckService()
	{

	}

}
