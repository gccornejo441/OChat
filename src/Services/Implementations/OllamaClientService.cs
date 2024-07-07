using OllamaClient.Services.Interfaces;

using OllamaSharp;
using OllamaSharp.Models;
using OllamaSharp.Models.Chat;
using OllamaSharp.Streamer;

namespace OllamaClient.Services.Implementations;
public class OllamaClientService : IOllamaClientService
{
	public static Uri OllamaUri = new("http://127.0.0.1:11434");

	public OllamaClientService()
	{

		var messageList = new List<Message>()
		{
			new()
			{
				Role = "role",
				Content = "content"
			}
		};

		ChatRequest chatRequest = new ChatRequest()
		{
			Model = "llama3",
			Messages = messageList
		};

	
	}

	OllamaApiClient client = new(OllamaUri,"ollama3");

	IResponseStreamer<CreateModelResponse> streamer { get; }

}