﻿using System.Drawing;
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
		string iconPath = @"C:\Users\gabriel.cornejo\source\repos\OChat\src\Assets\Images\ochat.ico";

		notifyIcon = new NotifyIcon
		{
			Icon = new Icon(iconPath),
			Visible = true,
			Text = "Your Application Name"
		};

		var contextMenu = new ContextMenuStrip();
		var showMenuItem = new ToolStripMenuItem("Show", null, OnShow);
		var exitMenuItem = new ToolStripMenuItem("Exit", null, OnExit);
		contextMenu.Items.Add(showMenuItem);
		contextMenu.Items.Add(exitMenuItem);
		notifyIcon.ContextMenuStrip = contextMenu;

		notifyIcon.DoubleClick += (sender, args) => ShowWindow();
	}

	private void OnShow(object sender, EventArgs e)
	{
		ShowWindow();
	}

	private void OnExit(object sender, EventArgs e)
	{
		notifyIcon.Visible = false;
		System.Windows.Application.Current.Shutdown();
	}

	private void ShowWindow()
	{
		var mainView = _mainViewFactory();
		mainView.Show();
		mainView.WindowState = WindowState.Normal;
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