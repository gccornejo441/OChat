using ReactiveUI;

namespace OllamaClient.Modals;
public class Modal : ReactiveObject
{
	private string? title;
	private string? message;
	public string Title { get => title; set => this.RaiseAndSetIfChanged(ref title,value); }
	public string Message { get => message; set => this.RaiseAndSetIfChanged(ref message,value); }
}
