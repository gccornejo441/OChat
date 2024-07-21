using System.Diagnostics;
using OllamaClient.Services.Interfaces;
using ReactiveUI;

namespace OllamaClient.Services;

/// <summary>
/// Provides a progress reporting service with ReactiveUI support.
/// </summary>
/// <typeparam name="T">Specifies the type of the progress report value.</typeparam>
public class ProgressService<T> : ReactiveObject, IProgressService<T>
{
	private readonly SynchronizationContext _synchronizationContext;
	private readonly Action<T>? _progressHandler;

	/// <summary>
	/// Initializes a new instance of the <see cref="ProgressService{T}"/> class.
	/// </summary>
	public ProgressService()
	{
		_synchronizationContext = SynchronizationContext.Current ?? ProgressStatics.DefaultContext;
		Debug.Assert(_synchronizationContext != null);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ProgressService{T}"/> class with a specified handler.
	/// </summary>
	/// <param name="handler">A handler to invoke for each reported progress value.</param>
	/// <exception cref="ArgumentNullException">Thrown when the handler is null.</exception>
	public ProgressService(Action<T> handler) : this() => _progressHandler = handler ?? throw new ArgumentNullException(nameof(handler));

	/// <summary>
	/// Raised for each reported progress value.
	/// </summary>
	public event EventHandler<T>? ProgressChanged;

	private bool _isIndeterminate;
	public bool IsIndeterminate
	{
		get => _isIndeterminate;
		set => this.RaiseAndSetIfChanged(ref _isIndeterminate, value);
	}

	private AppStatus _status;
	public AppStatus Status
	{
		get => _status;
		set => this.RaiseAndSetIfChanged(ref _status, value);
	}

	/// <summary>
	/// Reports a progress change.
	/// </summary>
	/// <param name="progressValue">The value of the updated progress.</param>
	protected virtual void OnReport(T progressValue)
	{
		var handler = _progressHandler;
		var progressChangedEvent = ProgressChanged;
		if (handler != null || progressChangedEvent != null)
		{
			if (progressValue is double progressAsDouble)
			{
				Status = progressAsDouble < 1.0 ? AppStatus.Running : AppStatus.Ready;
			}

			_synchronizationContext.Post(_ =>
			{
				handler?.Invoke(progressValue);
				progressChangedEvent?.Invoke(this, progressValue);
			}, progressValue);
		}
	}

	void IProgress<T>.Report(T value) => OnReport(value);

	/// <summary>
	/// Marks the progress as completed.
	/// </summary>
	public void Completed()
	{
		if (typeof(T) == typeof(double) && 1.0 is T progressValueAsCompleted)
		{
			OnReport(progressValueAsCompleted);
		}

		IsIndeterminate = false;
		Status = AppStatus.Ready;
	}
}

/// <summary>
/// Holds static values for <see cref="ProgressService{T}"/>.
/// </summary>
internal static class ProgressStatics
{
	internal static readonly SynchronizationContext DefaultContext = new();
}
