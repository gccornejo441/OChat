using System.Reactive;
using Microsoft.Extensions.Configuration;
using OllamaClient.Services;
using ReactiveUI;

namespace OllamaClient.Commons;
public class StatusBarCommands : ReactiveObject
{
	private readonly IProgressService<double> _progressService;
	private IEndpointService<EndpointStatus> _endpointService;
	private IConfiguration _configuration;

	public StatusBarCommands(IProgressService<double> progressService, 
		IConfiguration configuration, IEndpointService<EndpointStatus> endpointService)
	{
		_progressService = progressService;
		_configuration = configuration;
		_endpointService = endpointService;

	}																	   

	public void TriggerProgressBar()
	{
		_endpointService.Report(EndpointStatus.Available);
	}

	public void SetStatusReady()
	{

		//_progressService.Report(0.5);

		//_progressService.Completed();

	}

	public void DisconnectEndpoint()
	{
		//_progressService.Report(0.0);
		_endpointService.Report(EndpointStatus.Unavailable);
	}

	public void CheckService()
	{

	}

}
