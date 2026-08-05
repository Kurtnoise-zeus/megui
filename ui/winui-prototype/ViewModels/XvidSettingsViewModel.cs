using System.ComponentModel;
using System.Runtime.CompilerServices;
using WinUIPrototype.Models;

namespace WinUIPrototype.ViewModels
{
    public class XvidSettingsViewModel : INotifyPropertyChanged
    {
        public Models.XvidSettings Settings { get; private set; } = new Models.XvidSettings();

        public int ReactionDelayFactor
        {
            get => Settings.ReactionDelayFactor;
            set { Settings.ReactionDelayFactor = value; OnPropertyChanged(); }
        }

        public int AveragingPeriod
        {
            get => Settings.AveragingPeriod;
            set { Settings.AveragingPeriod = value; OnPropertyChanged(); }
        }

        public int RateControlBuffer
        {
            get => Settings.RateControlBuffer;
            set { Settings.RateControlBuffer = value; OnPropertyChanged(); }
        }

        public bool Turbo
        {
            get => Settings.Turbo;
            set { Settings.Turbo = value; OnPropertyChanged(); }
        }

        public int MinPQuant
        {
            get => Settings.MinPQuant;
            set { Settings.MinPQuant = value; OnPropertyChanged(); }
        }

        public int MaxPQuant
        {
            get => Settings.MaxPQuant;
            set { Settings.MaxPQuant = value; OnPropertyChanged(); }
        }

        public void LoadFromModel(Models.XvidSettings model)
        {
            Settings = model;
            OnPropertyChanged(string.Empty);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
