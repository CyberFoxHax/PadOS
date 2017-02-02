using System;
using System.Collections.Generic;

namespace PadOS.Views.MainPanel {
	public class MainPanelButton {

		public string ImageUri { get; set; }
		public string Key { get; set; }
		public ContentType ContentType { get; set; }

		public void Activate(){
			switch (ContentType){
				case ContentType.Window: OpenWindow(); break;
			}
		}

		private static readonly Dictionary<string, Type> Windows = new Dictionary<string, Type>{
			{"settings", typeof(Settings.Settings)},
			{"osk", typeof(GamePadOSK.Osk)}
		};

		private void OpenWindow(){
			if (Windows.ContainsKey(Key) == false) return;
			var window = Windows[Key].CreateInstance<System.Windows.Window>();

			if (window == null) return;
			window.Show();
		}
	}
}
