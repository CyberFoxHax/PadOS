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

            BlockNavigator.AddNavigationExitHandler(FunctionButtonList, FunctionButtonList_NavigationExit);
            BlockNavigator.AddNavigationEnterHandler(FunctionButtonList, FunctionButtonList_NavigationEnter);
            BlockNavigator.AddNavigationExitHandler(DialAreaControl, DialAreaControl_NavigationExit);
            BlockNavigator.AddNavigationEnterHandler(DialAreaControl, DialAreaControl_NavigationEnter);
            BlockNavigator.AddConfirmClickHandler(DialAreaControl, DialAreaControl.DialArea_ConfirmClick);
            DialAreaControl.ItemPicked += DialArea_ItemPicked;
            FunctionButtonList.ItemSelected += FunctionButtonList_ItemSelected;

            var ctx = new SaveData.SaveData();
            FunctionButtonList.LoadList(ctx.Functions.ToList());
            Profiles_ComboBox.ItemsSource = ctx.Profiles.Select(p => new ComboBoxItemContainer {
                Text = p.Name,
                DataContext = p
            }).ToList();
            var profile = ctx.Profiles.First();
            var panelButtons = ctx.PanelButtons.Where(p => p.Profile.Id == profile.Id);
            var formData = new EditorFormData {
                Profile = profile,
                PanelButtons = panelButtons
            };
            LoadProfile(formData);
        }

        private class EditorFormData {
            public SaveData.Models.Profile Profile { get; set; }
            public System.Collections.Generic.IEnumerable<SaveData.Models.PanelButton> PanelButtons { get; set; }
        }

        private bool _hasUnsavedChanges = false;
        private EditorFormData _editorFormData;

        private void LoadProfile(EditorFormData data) {
            _editorFormData = data;
            DialAreaControl.LoadPanel(data.PanelButtons);
        }

        private enum DialogueResult {
            Cancel,
            Yes,
            No
        }

        private bool ConfirmLeave() {
            if (_hasUnsavedChanges == false)
                return true;

            var dialogResult = DialogueResult.Cancel; // TODO: Make dialogue
            switch (dialogResult) {
                case DialogueResult.Cancel:
                    return false;
                case DialogueResult.Yes:
                    return true;
                case DialogueResult.No:
                    return true;
            }

            return false;
        }

        private void Profiles_ComboBox_ItemClicked(object sender, ComboBoxItemContainer item) {
            if (ConfirmLeave() == false)
                return;

            var ctx = new SaveData.SaveData();
            var profile = (SaveData.Models.Profile)item.DataContext;
            var panelButtons = ctx.PanelButtons.Where(p => p.Profile.Id == profile.Id);
            var formData = new EditorFormData {
                Profile = profile,
                PanelButtons = panelButtons
            };
            LoadProfile(formData);
            _hasUnsavedChanges = false;

            Profiles_ComboBox.Close();
            BlockNavigator.NavigateBack((FrameworkElement)sender);
        }

        private void Window_CancelClick(object sender, EventArgs args) {
            if (ConfirmLeave() == false)
                return;
            Navigator.NavigateBack();
        }

        private void CreateNewFunction_ButtonClick(object sender, MouseButtonEventArgs e) {
            // change view to function creation
        }


        // Interopation between FunctionButtonsList and DialArea

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
                _hasUnsavedChanges = true;
            }
            else if(_formState == FormState.FunctionButtonsMode) {
                _functionsSelection = model;
                _hasUnsavedChanges = true;
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
