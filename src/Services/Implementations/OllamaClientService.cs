using System.Net.Http;

using OllamaClient.Services.Interfaces;

using OllamaSharp;
using OllamaSharp.Models;
using OllamaSharp.Streamer;

namespace OllamaClient.Services.Implementations;
public class OllamaClientService : IOllamaClientService
{
    public static Uri OllamaUri = new Uri("http://127.0.0.1:11434");

	public OllamaClientService()
    {
     
    }

	OllamaApiClient client = new OllamaApiClient(OllamaUri, "ollama3");
    IResponseStreamer<CreateModelResponse> streamer { get; }
 
}