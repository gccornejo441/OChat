﻿namespace OllamaClient.Services;

public enum EndpointStatus
{
	Available,
	Unavailable,
	Error,
}

/// <summary>
/// Represents a service for managing Ollama endpoints.
/// </summary>
public interface IEndpointService<T> : IProgress<T>
{
	EndpointStatus Status { get; }
	bool IsIndeterminate { get; }

	/// <summary>
	/// Occurs when the status of the endpoint changes.
	/// </summary>
	event EventHandler<T> StatusChanged;
}