using PadOS.Commands.FunctionButtons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PadOS.SaveData.Models;

namespace PadOS.Views.MainPanelEditor {
    public partial class FunctionButtonList : UserControl {
        public FunctionButtonList() {
            InitializeComponent();

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                return;

            ItemsControl.Items.Clear();
        }

        public event Action<object, FunctionViewModel> ItemClicked;

        public void LoadList(List<Function> functions) {
            var functionButtons = functions.Select(p => new FunctionViewModel {
                Function = p,
                FunctionButton = new FunctionButton {
                    FunctionType = p.FunctionType,
                    Title = p.Title,
                    ImageUri = p.ImageUrl != null ? new Uri(Utils.ResourcesPath + p.ImageUrl) : null
                }
            });
            ItemsControl.Items.Clear();
            ItemsControl.ItemsSource = functionButtons;
        }

        public FrameworkElement GetFirstItemElement() {
            var items = ItemsControl.Items;
            var item = ItemsControl.ItemContainerGenerator.ContainerFromItem(items.Cast<object>().First());
            var child = System.Windows.Media.VisualTreeHelper.GetChild(item, 0) as FrameworkElement;
            return child;
        }

        private void Item_OnClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var control = sender as FrameworkElement;
            var functionButton = control.DataContext as FunctionViewModel;
            ItemClicked?.Invoke(sender, functionButton);
        }
    }
}
