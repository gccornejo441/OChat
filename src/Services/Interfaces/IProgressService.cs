using System.ComponentModel;

namespace OllamaClient.Services;

public enum AppStatus
{
	Running,
	Ready
}
public interface IProgressService<T> : IProgress<T>, INotifyPropertyChanged
{
	public event EventHandler<T> ProgressChanged;
	public bool IsIndeterminate { get; set; }
	public AppStatus Status { get; set; }

	void Completed();
}
