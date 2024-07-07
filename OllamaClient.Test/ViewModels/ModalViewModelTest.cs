using OllamaClient.Modals;
using OllamaClient.ViewModels;

namespace OllamaClient.Test.ViewModels;
public class ModalViewModelTest
{
	[Fact]
	public void Modal_SetValue_RaisesPropertyChanged()
	{
		var viewModel = new ModalViewModel();
		bool propertyChangedRaised = false;

		viewModel.PropertyChanged += (sender,e) =>
		{
			if (e.PropertyName == nameof(viewModel.Modal))
			{
				propertyChangedRaised = true;
			}
		};

		viewModel.Modal = new Modal(); 

		Assert.True(propertyChangedRaised);
	}

	[Fact]
	public void InitialState_IsCorrect()
	{
		var viewModel = new ModalViewModel();

		Assert.Null(viewModel.Modal);
		Assert.NotNull(viewModel.CloseCommand);
	}
}
