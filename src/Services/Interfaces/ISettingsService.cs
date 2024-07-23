using ReactiveUI;

namespace OllamaClient.Services;

public interface ISettingsService 
{
    void Save();
    bool IsHealthy();

    string GetVersionNumber();

}