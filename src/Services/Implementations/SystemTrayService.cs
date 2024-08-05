using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;

namespace OllamaClient.Services;
public class SystemTrayService : IDisposable
{
	private NotifyIcon notifyIcon;
	private Func<MainView> _mainViewFactory;

	public SystemTrayService(Func<MainView> mainViewFactory)
	{
		_mainViewFactory = mainViewFactory;
		InitializeNotifyIcon();
	}

	private void InitializeNotifyIcon()
	{
		string resourceName = "OllamaClient.Assets.Images.ochat.ico";
		var assembly = Assembly.GetExecutingAssembly().GetManifestResourceNames().ToList();
		using (Stream iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
		{
			if (iconStream == null)
			{
				throw new ArgumentException($"Resource '{resourceName}' not found.");
			}

			notifyIcon = new NotifyIcon
			{
				Icon = new Icon(iconStream),
				Visible = true,
				Text = "O Chat"
			};
		}

		var contextMenu = new ContextMenuStrip();
		var exitMenuItem = new ToolStripMenuItem("Exit", null, OnExit);
		contextMenu.Items.Add(exitMenuItem);
		notifyIcon.ContextMenuStrip = contextMenu;
	}

	private void OnExit(object sender, EventArgs e)
	{
		notifyIcon.Visible = false;
		System.Windows.Application.Current.Shutdown();
	}

	public void HandleWindowStateChange(object sender, EventArgs e)
	{
		var mainView = _mainViewFactory();

		if (mainView.WindowState == WindowState.Minimized)
		{
			mainView.Hide();
		}
	}

	public void Dispose()
	{
		notifyIcon.Dispose();
	}
}