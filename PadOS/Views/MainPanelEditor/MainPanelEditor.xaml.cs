using PadOS.Input.BlockNavigator;
using PadOS.Navigation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PadOS.Views.MainPanelEditor {
    public partial class MainPanelEditor{
		public MainPanelEditor() {
			InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
				return;

            DialAreaControl.ItemPicked += DialArea_ItemPicked;
            var ctx = new SaveData.SaveData();
            Profiles_ComboBox.ItemsSource = ctx.Profiles.Select(p=>p.Name).ToList();
            var profile = ctx.Profiles.First();
            var saveData = ctx.PanelButtons.Where(p => p.Profile.Id == profile.Id);
            DialAreaControl.LoadPanel(saveData);
            FunctionButtonList.LoadList(ctx.Functions.ToList());

            InitialState();
            
        }

        private void Window_CancelClick(object sender, EventArgs args) {
            Navigator.NavigateBack();
        }

        private void CreateNew_ButtonClick(object sender, MouseButtonEventArgs e) {
            // change view to function creation
        }


        // Interopation between FunctionButtonsList and DialArea

        private void InitialState() {
            BlockNavigator.AddNavigationExitHandler(FunctionButtonList, FunctionButtonList_NavigationExit);
            BlockNavigator.AddNavigationEnterHandler(FunctionButtonList, FunctionButtonList_NavigationEnter);
            BlockNavigator.AddNavigationExitHandler(DialAreaControl, DialAreaControl_NavigationExit);
            BlockNavigator.AddNavigationEnterHandler(DialAreaControl, DialAreaControl_NavigationEnter);
            FunctionButtonList.ItemSelected += FunctionButtonList_ItemSelected;
            BlockNavigator.AddConfirmClickHandler(DialAreaControl, DialAreaControl.DialArea_ConfirmClick);
        }

        private FormState _formState = FormState.Normal;
        private FunctionViewModel _dialAreaSelection;
        private FunctionViewModel _functionsSelection;

        private void DialArea_ItemPicked(FrameworkElement sender, FunctionViewModel obj) {
            if(_formState == FormState.DialAreaMode) {
                _dialAreaSelection = DialAreaControl.Selection;
                DialAreaControl.Disable();
                BlockNavigator.SetFocus(sender, FunctionButtonList.GetFirstItemElement());
            }

            else if(_formState == FormState.FunctionButtonsMode){
                DialAreaControl.ReplaceSelectedItem(_functionsSelection);
                DialAreaControl.Reset();
                DialAreaControl.Disable();
                BlockNavigator.SetFocus(sender, FunctionButtonList.GetFirstItemElement());
            }
        }

        private void FunctionButtonList_ItemSelected(object sender, FunctionViewModel model) {
            BlockNavigator.SetFocus(sender as FrameworkElement, DialAreaControl);
            BlockNavigator.EnterNestedNavigator(DialAreaControl);

            if (_formState == FormState.DialAreaMode) {
                DialAreaControl.ReplaceSelectedItem(model);
            }
            else if(_formState == FormState.FunctionButtonsMode) {
                _functionsSelection = model;
            }
            DialAreaControl.Enable();
        }

        private void FunctionButtonList_NavigationEnter(object sender, EventArgs args) {
            if (_formState == FormState.Normal)
                _formState = FormState.FunctionButtonsMode;
        }

        private void FunctionButtonList_NavigationExit(object sender, EventArgs args) {
            _formState = FormState.Normal;
        }

        private void DialAreaControl_NavigationEnter(object sender, EventArgs args) {
            if (_formState == FormState.Normal)
                _formState = FormState.DialAreaMode;
        }

        private void DialAreaControl_NavigationExit(object sender, EventArgs args) {
            _formState = FormState.Normal;
        }

    }
}
