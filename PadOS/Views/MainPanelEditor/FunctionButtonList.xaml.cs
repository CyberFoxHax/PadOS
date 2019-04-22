using PadOS.Commands.FunctionButtons;
using PadOS.SaveData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PadOS.Views.MainPanelEditor {
    public partial class FunctionButtonList : UserControl {
        public FunctionButtonList() {
            InitializeComponent();
        }

        public void LoadList(List<Function> functions) {
            var functionButtons = functions.Select(p=> new FunctionButton {
                FunctionType = p.FunctionType,
                Title = p.Title,
                ImageUri = p.ImageUrl != null ? new Uri("pack://application:,,,/PadOS;component/Resources/" + p.ImageUrl) : null
            }).ToList();
            ItemsControl.Items.Clear();
            ItemsControl.ItemsSource = functionButtons;
        }

        public FrameworkElement GetFirstItemElement() {
            var items = ItemsControl.Items;
            var item = ItemsControl.ItemContainerGenerator.ContainerFromItem(items.Cast<object>().First());
            var child = System.Windows.Media.VisualTreeHelper.GetChild(item, 0) as FrameworkElement;
            return child;
        }

    }
}
