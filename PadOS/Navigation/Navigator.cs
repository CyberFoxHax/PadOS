using System;
using System.Collections.Generic;
using System.Windows;
using PadOS.Input;
using PadOS.Views.MainPanel;
using XInputDotNetPure;

namespace PadOS.Navigation {
	public static class Navigator {

		public static void Initialize(){
			_mainPanel = new MainPanel();
			GamePadInput.StaticInputInstance.ButtonGuideDown += XInputOnButtonGuideDown;
		}

		private static void XInputOnButtonGuideDown(int player, GamePadState state){
			if (CurrentWindow != null)
				App.GlobalDispatcher.Invoke(CloseWindow);
			else
				App.GlobalDispatcher.Invoke(OpenMainPanel);
		}

		private static readonly Dictionary<Type, Window> Windows = new Dictionary<Type, Window>();
		public static Window CurrentWindow { get; private set; }
		private static MainPanel _mainPanel;

		public static void CloseWindow() {
			if (ReferenceEquals(CurrentWindow, _mainPanel))
				CurrentWindow.Hide();
			else
				CurrentWindow.Close();

			CurrentWindow = null;
		}

		public static void OpenMainPanel(){
			CurrentWindow = _mainPanel;
			_mainPanel.Highlight.Visibility = Visibility.Hidden;
			_mainPanel.Show();
		}

		public static void ClearCache(Type type){
			Windows.Remove(type);
		}

		public static void ClearCache<T>() where T:Window{
			ClearCache(typeof(T));
		}

		public static object OpenWindow(Type type, bool cache = false) {
			var instance = Activator.CreateInstance(type);
			if (CurrentWindow != null)
				CloseWindow();
			CurrentWindow = (Window)instance;
			CurrentWindow.Show();
			if (cache == false)
				return instance;

			if (Windows.ContainsKey(type))
				Windows[type] = (Window) instance;

			return instance;
		}

		public static T OpenWindow<T>(bool cache = false) where T : Window{
			return (T)OpenWindow(typeof (T), cache);
		}
	}
}
