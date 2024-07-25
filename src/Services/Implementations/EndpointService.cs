
using System.Net.Http;
using System.Reflection;
using ReactiveUI;

namespace OllamaClient.Services;
public class EndpointService : ReactiveObject, IEndpointService<EndpointStatus>
{
	private readonly HttpClient _client;
	private readonly string _url;
	private EndpointStatus _status;
	private readonly CancellationTokenSource _cancellationTokenSource;
	private bool _isIndeterminate;
	public event EventHandler<EndpointStatus> StatusChanged;

	public EndpointService(string url)
	{
		_client = new HttpClient();
		_url = url;
		_cancellationTokenSource = new CancellationTokenSource();
		Task.Run(() => MonitorEndpoint(_cancellationTokenSource.Token), _cancellationTokenSource.Token);

		var assembly = Assembly.GetEntryAssembly();
	}

	public bool IsIndeterminate
	{
		get => _isIndeterminate;
		set => this.RaiseAndSetIfChanged(ref _isIndeterminate, value);
	}

	// When the status property is set 
	// RaiseAndSetIfChanged updates the property 
	// and notifies any observers (Report).
	public EndpointStatus Status
	{
		get => _status;
		set
		{
			this.RaiseAndSetIfChanged(ref _status, value);
		}
	}

	public EndpointStatus CurrentEndpointState()
	{
		try
		{
			var response = _client.GetAsync(_url).Result;
			if (response.IsSuccessStatusCode)
			{
				return EndpointStatus.Available;
			}
			else
			{
				return EndpointStatus.Unavailable;
			}
		}
		catch (Exception)
		{
			return EndpointStatus.Error;
		}
	}

	/// <summary>
	/// Reports the current status of the endpoint by invoking the StatusChanged event.
	/// This method is typically called when the status of the endpoint changes.
	/// </summary>
	/// <param name="value"></param>
	/// </summary>
	public void Report(EndpointStatus status)
	{
		if (status != null)
		{
			Status = status == EndpointStatus.Available ? EndpointStatus.Available : EndpointStatus.Unavailable;
		}

		StatusChanged?.Invoke(this, status);
	}

	private async Task MonitorEndpoint(CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			Status = CurrentEndpointState();
			await Task.Delay(5000, cancellationToken);
		}
	}

}