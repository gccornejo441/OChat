/// <copyright file="SettingsService.cs">
/// © 2023 gabecornejo.com, Inc. All rights reserved.
/// </copyright>

using OllamaClient.Core;
using OllamaClient.Services.Interfaces;
using ReactiveUI;

public partial class ServicesService : ReactiveObject, ISettingsService
{
    private bool isLoaded;
    private readonly string assemblyVersion;
    public const string ProductName = "OllamaClient";
    public const string AssemblyName = ProductName + ".dll";

    public ServicesService()
    {
        assemblyVersion = AssemblyUtilities.GetAssemblyVersion(AssemblyName).ToString();
    }

    public bool IsHealthy()
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }
}
