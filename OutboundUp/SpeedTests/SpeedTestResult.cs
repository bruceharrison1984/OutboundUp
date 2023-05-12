namespace OutboundUp.SpeedTests
{
    public class SpeedTestResult
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Timestamp of when the test was ran
        /// </summary>
        public long UnixTimestampMs { get; set; }

        /// <summary>
        /// Was the overall test successful
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Hostname of the server that was used during the test
        /// </summary>
        public string? ServerHostName { get; set; }

        /// <summary>
        /// Latency in milliseconds
        /// </summary>
        public double UploadLatencyAverage { get; set; }

        /// <summary>
        /// The highest latency from a given test set
        /// </summary>
        public double UploadLatencyHigh { get; set; }

        /// <summary>
        /// The lowest latency from a given test set
        /// </summary>
        public double UploadLatencyLow { get; set; }

        /// <summary>
        /// Latency in milliseconds
        /// </summary>
        public double DownloadLatencyAverage { get; set; }

        /// <summary>
        /// The highest latency from a given test set
        /// </summary>
        public double DownloadLatencyHigh { get; set; }

        /// <summary>
        /// The lowest latency from a given test set
        /// </summary>
        public double DownloadLatencyLow { get; set; }

        /// <summary>
        /// Average ping in milliseconds
        /// </summary>
        public double PingAverage { get; set; }

        /// <summary>
        /// The highest ping from a given test set
        /// </summary>
        public double PingHigh { get; set; }

        /// <summary>
        /// The lowest ping from a given test set
        /// </summary>
        public double PingLow { get; set; }

        /// <summary>
        /// Download speed in Mbps
        /// </summary>
        public double DownloadSpeed { get; set; }

        /// <summary>
        /// Upload speed in Mbps
        /// </summary>
        public double UploadSpeed { get; set; }
    }
}
