﻿using System;
using Eto;
using Eto.Forms;
using RepoZ.Api.Win.Git;
using RepoZ.Api.Win.IO;
using RepoZ.Api.Git;
using RepoZ.Api.IO;
using TinyIoC;
using RepoZ.Api.Win.PInvoke;
using RepoZ.Api.Common;

namespace RepoZ.UI.Win
{
	public class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			var container = TinyIoCContainer.Current;

			container.Register<MainForm>().AsSingleton();
			container.Register<IRepositoryInformationAggregator>((c, p) => c.Resolve<MainForm>());

			container.Register<IRepositoryMonitor, DefaultRepositoryMonitor>().AsSingleton();
			container.Register<WindowsExplorerHandler>().AsSingleton();

			container.Register<IErrorHandler, UIErrorHandler>();
			container.Register<IRepositoryActionProvider, WindowsRepositoryActionProvider>();
			container.Register<IRepositoryObserverFactory, DefaultRepositoryObserverFactory>();
			container.Register<IRepositoryReader, WindowsRepositoryReader>();
			container.Register<IRepositoryWriter, WindowsRepositoryWriter>();
			container.Register<IPathProvider, DefaultDriveEnumerator>();
			container.Register<IPathCrawler, GravellPathCrawler>();
			container.Register<IPathCrawlerFactory, DefaultPathCrawlerFactory>();

			var application = new Application(Platform.Detect);
			var mainForm = container.Resolve<MainForm>();

			var explorerHandler = container.Resolve<WindowsExplorerHandler>();

			var timer = new UITimer();
			timer.Interval = 0.5;
			timer.Elapsed += (s, e) => explorerHandler.Pulse();
			timer.Start();

			application.Run(mainForm);
		}
	}
}
