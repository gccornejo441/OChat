﻿using System.Reactive.Linq;

using Moq;

using OllamaClient.ViewModels;

using OllamaSharp;
using OllamaSharp.Models;

namespace OllamaClient.Test.ViewModels;
public class MainViewModelTests
{
	private readonly Mock<IOllamaApiClient> mockApiClient;
	private readonly MainViewModel viewModel;

	public MainViewModelTests()
	{
		mockApiClient = new Mock<IOllamaApiClient>();
		viewModel = new MainViewModel(mockApiClient.Object);
	}


	[Fact]
	public async Task GetModelsCommand_Should_PopulateModels_OnSuccess()
	{
		var mockModels = new List<Model> { new Model { Name = "Model1" },new Model { Name = "Model2" } };
		mockApiClient.Setup(api => api.ListLocalModels(CancellationToken.None)).ReturnsAsync(mockModels);

		await viewModel.GetModelsCommand.Execute();

		Assert.Equal(2,viewModel.Models.Count);
		Assert.Contains("Model1",viewModel.Models);
		Assert.Contains("Model2",viewModel.Models);
	}
}