using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using WinUIPrototype.Views;

namespace WinUIPrototype
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            RootNav.SelectionChanged += RootNav_SelectionChanged;
            ContentFrame.Navigate(typeof(XvidSettingsPage));
        }

        private void RootNav_SelectionChanged(object sender, NavigationViewSelectionChangedEventArgs e)
        {
            if (e.SelectedItemContainer is NavigationViewItem item)
            {
                var tag = item.Tag?.ToString();
                switch (tag)
                {
                    case "encoders":
                        ContentFrame.Navigate(typeof(XvidSettingsPage));
                        break;
                    case "settings":
                        ContentFrame.Navigate(typeof(XvidSettingsPage));
                        break;
                    default:
                        ContentFrame.Navigate(typeof(XvidSettingsPage));
                        break;
                }
            }
        }
    }
}
