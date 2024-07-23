using OllamaClient.Services;
using ReactiveUI;

namespace OllamaClient.ViewModels;
public class EndpointViewModel : ReactiveObject
{

	private readonly IEndpointService<EndpointStatus> _endpointService;
	private EndpointStatus _status;

	public EndpointViewModel(IEndpointService<EndpointStatus> endpointService)
	{
		_endpointService = endpointService;
		_endpointService.StatusChanged += OnProgressChanged;
	}

	public EndpointStatus Status
	{
		get => _status;
		set => this.RaiseAndSetIfChanged(ref _status, value);
	}

	private void OnProgressChanged(object? sender, EndpointStatus e)
	{
		Status = e;
	}
}
