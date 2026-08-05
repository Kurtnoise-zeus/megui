using Microsoft.UI.Xaml;

namespace WinUIPrototype
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _ = new MainWindow().Activate();
        }
    }
}
