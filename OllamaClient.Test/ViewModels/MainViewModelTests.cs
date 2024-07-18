using System.Reactive.Linq;
using Moq;
using OllamaClient.Services.Interfaces;
using OllamaClient.ViewModels;
using OllamaSharp;
using OllamaSharp.Models;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive;

namespace OllamaClient.Test.ViewModels
{
	public class MainViewModelTests
	{
		private readonly Mock<IOllamaApiClient> mockApiClient;
		private readonly Mock<IModalService> mockObservableModel;
		private readonly MainViewModel viewModel;

		public MainViewModelTests()
		{
			mockApiClient = new Mock<IOllamaApiClient>();
			mockObservableModel = new Mock<IModalService>();
			viewModel = new MainViewModel(mockApiClient.Object,mockObservableModel.Object);
		}

		[Fact]
		public async Task GetModelsCommand_Should_PopulateModels_OnSuccess()
		{
			try
			{
				var mockModels = new List<Model> { new Model { Name = "Model1" },new Model { Name = "Model2" } };
				mockApiClient.Setup(api => api.ListLocalModels(CancellationToken.None)).ReturnsAsync(mockModels);

				await viewModel.GetModelsCommand.Execute();

				Assert.Equal(2,viewModel.Models.Count);
				Assert.Contains("Model1",viewModel.Models);
				Assert.Contains("Model2",viewModel.Models);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}
		}
	}
}
