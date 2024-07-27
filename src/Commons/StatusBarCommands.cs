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
		IConfiguration configuration)
	{
		_progressService = progressService;
		_configuration = configuration;

		var url = _configuration.GetValue<string>("JSONPlaceholderSetting:ApiBaseUrl");
	   

	}																	   

	public void TriggerProgressBar()
	{
		throw new NotImplementedException();

	}

	public void SetStatusReady()
	{


		//_progressService.Report(0.5);

		//_progressService.Completed();

	}

	public void DisconnectEndpoint()
	{
		//_progressService.Report(0.0);
	}

	public void CheckService()
	{

	}

}
