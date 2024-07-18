using System.Windows.Controls;
using OllamaClient.ViewModels;

namespace OllamaClient.Controls;
/// <summary>
/// Interaction logic for StatusBarView.xaml
/// </summary>
public partial class StatusBarView : UserControl
{
	public StatusBarView(StatusBarViewModel statusBarViewModel)
	{
		InitializeComponent();
		DataContext = statusBarViewModel;
	}

	//public StatusBarView(StatusBarViewModel statusBarViewModel) : this()
	//{
	//	DataContext = statusBarViewModel;
	//}
}
