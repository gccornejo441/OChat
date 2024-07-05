using OllamaClient.Services.Interfaces;

using OllamaSharp;

namespace OllamaClient.Services.Implementations;
public class OllamaClientService : IOllamaClientService
{
    public static Uri OllamaUri = new Uri("http://127.0.0.1:11434");
    public static OllamaApiClient OllamaApi = new(OllamaUri);

    public OllamaClientService()
    {
        OllamaUri = new Uri(Environment.GetEnvironmentVariable("OLLAMA_API") ?? OllamaUri.ToString());
        OllamaApi = new OllamaApiClient(OllamaUri);
    }
}