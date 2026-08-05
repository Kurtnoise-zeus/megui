namespace WinUIPrototype.Models
{
    public class XvidSettings
    {
        public int ReactionDelayFactor { get; set; } = 16;
        public int AveragingPeriod { get; set; } = 100;
        public int RateControlBuffer { get; set; } = 100;
        public bool Turbo { get; set; } = true;

        public int MinPQuant { get; set; } = 2;
        public int MaxPQuant { get; set; } = 31;

        // Add additional properties as needed to mirror existing repo settings
    }
}
