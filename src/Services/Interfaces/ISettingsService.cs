using ReactiveUI;

namespace OllamaClient.Services.Interfaces;

public interface ISettingsService 
{
    void Save();
    bool IsHealthy();

    string GetVersionNumber();

}