namespace OutboundUp.Models
{
    public class OutboundUpOptions
    {
        /// <summary>
        /// How many minutes should be between speed test runs
        /// </summary>
        public int SpeedTestIntervalMinutes { get; set; } = 30;

        /// <summary>
        /// How frequently should old speed test results be pruned
        /// </summary>
        public int DataCleanupIntervalHours { get; set; } = 24;

        /// <summary>
        /// How many days old should an entry be before it is pruned
        /// </summary>
        public int StaleEntryTTLDays { get; set; } = 90;
    }
}
