using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using System.Text.Json;
using WinUIPrototype.ViewModels;

namespace WinUIPrototype.Views
{
    public sealed partial class XvidSettingsPage : Page
    {
        public XvidSettingsViewModel ViewModel { get; } = new XvidSettingsViewModel();

        private readonly string DemoPath = Path.Combine("packages", "video", "xvid", "xvid_settings_demo.json");

        public XvidSettingsPage()
        {
            this.InitializeComponent();
        }

        private async void Save_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                var dir = Path.GetDirectoryName(DemoPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var json = JsonSerializer.Serialize(ViewModel.Settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(DemoPath, json);
                var dlg = new ContentDialog { Title = "Saved", Content = $"Saved to {DemoPath}", CloseButtonText = "OK" };
                await dlg.ShowAsync();
            }
            catch (System.Exception ex)
            {
                var dlg = new ContentDialog { Title = "Error", Content = ex.Message, CloseButtonText = "OK" };
                await dlg.ShowAsync();
            }
        }

        private async void Load_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                if (!File.Exists(DemoPath))
                {
                    var dlg = new ContentDialog { Title = "Not found", Content = $"File {DemoPath} not found (save first).", CloseButtonText = "OK" };
                    await dlg.ShowAsync();
                    return;
                }
                var json = File.ReadAllText(DemoPath);
                var model = JsonSerializer.Deserialize<Models.XvidSettings>(json);
                if (model != null) ViewModel.LoadFromModel(model);
            }
            catch (System.Exception ex)
            {
                var dlg = new ContentDialog { Title = "Error", Content = ex.Message, CloseButtonText = "OK" };
                await dlg.ShowAsync();
            }
        }
    }
}
