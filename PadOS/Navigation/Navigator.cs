using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using PadOS.Input.GamePadInput;
using PadOS.Views.CircleDial;
using XInputDotNetPure;

namespace PadOS.Navigation
{
    public static class Navigator {

		public static void Initialize(){
			GamePadInput.StaticInputInstance.ButtonGuideDown += XInputOnButtonGuideDown;

            _profileManager = new ProfileSwitcher.ProfileManager();
            _profileManager.Init();

            _mainPanel = new CircleDial();
        }

        private static ProfileSwitcher.ProfileManager _profileManager;

        public static void ToggleMainPanel() {
            if (CurrentWindow != null) {
                _profileManager.ProfileEnabled = true;
                App.GlobalDispatcher.Invoke(CloseWindow);
                _navigationHistory.Clear();
            }
            else {
                _profileManager.ProfileEnabled = false;
                App.GlobalDispatcher.Invoke(OpenMainPanel);
            }
        }


        private static void XInputOnButtonGuideDown(int player, GamePadState state){
            ToggleMainPanel();
        }

		private static readonly Dictionary<Type, Window> Windows = new Dictionary<Type, Window>();
		public static Window CurrentWindow { get; private set; }
		private static CircleDial _mainPanel;
        private static Stack<Window> _navigationHistory = new Stack<Window>();

        public static SaveData.Models.Profile GetCurrentProfile() {
            return _profileManager.CurrentProfile?._profile;
        }

		public static void CloseWindow() {
            if (CurrentWindow == _mainPanel){
                if(CurrentWindow is IHideable hideable)
                    hideable.Hide();
                else
                    CurrentWindow.Hide();
            }
            else
                CurrentWindow.Close();
            CurrentWindow = null;
		}

        public static void EnableProfile() {
            _profileManager.ProfileEnabled = true;
        }

		public static void OpenMainPanel(){
            _profileManager.ProfileEnabled = false;
            CurrentWindow = _mainPanel;
            _navigationHistory.Push(CurrentWindow);
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
            GamePadInput.StaticInputInstance.AwaitReset = true;
            var instance = Activator.CreateInstance(type);
            if (CurrentWindow != null)
				CloseWindow();
			CurrentWindow = (Window)instance;
			CurrentWindow.Show();
            _navigationHistory.Push(CurrentWindow);
			if (cache == false)
				return instance;

			if (Windows.ContainsKey(type))
				Windows[type] = (Window) instance;

			return instance;
		}

		public static T OpenWindow<T>(bool cache = false) where T : Window{
			return (T)OpenWindow(typeof(T), cache);
		}

        public static Window NavigateBack() {
            if (_navigationHistory.Count == 1)
                return null;

            GamePadInput.StaticInputInstance.AwaitReset = true;

            CurrentWindow.Close();
            _navigationHistory.Pop();
            var window = _navigationHistory.Peek();
            {
                // for whatever reason, a previously hidden window will cause problems, intead just create a new copy and show that.
                var instance = Activator.CreateInstance(window.GetType());
                CurrentWindow = (Window)instance;
                CurrentWindow.Show();
            }

            return window;
        }

		public static void Shutdown(){
			GamePadInput.StaticInputInstance.ButtonGuideDown -= XInputOnButtonGuideDown;
			GamePadInput.StaticInputInstance.Dispose();
			_mainPanel.Close();
			CurrentWindow?.Close();
			Windows.Clear();
		}

	}
}
