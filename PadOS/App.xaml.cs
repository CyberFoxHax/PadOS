using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using PadOS.Navigation;
using PadOS.SaveData;

namespace PadOS {
	public partial class App{
		public static Dispatcher GlobalDispatcher { get; private set; }

		public App(){
			GlobalDispatcher = Dispatcher;
		}

		protected override void OnStartup(System.Windows.StartupEventArgs e){
			var ctx = new SaveData.SaveData();
			ctx.DeleteIfExists();
			ctx.CreateDb();
			ctx.InsertDefault();

			base.OnStartup(e);
			var systray = new Views.SystemTray();

			Navigator.Initialize();
			if (Debugger.IsAttached == false)
				return;

			Navigator.OpenMainPanel();
		}
	}
}
