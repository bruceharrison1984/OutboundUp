namespace OutboundUp.SpeedTests.Ookla
{
    /// <summary>
    /// Test results as returned by Ookla. 
    /// Ookla CLI returns all bandwidth measurements in bytes when using json or csv output regardless of selected output format.
    /// </summary>
    public class OoklaTestResults
    {
        public string Type { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public PingInfo Ping { get; set; }
        public TransferInfo Download { get; set; }
        public TransferInfo Upload { get; set; }
        public double PacketLoss { get; set; }
        public string Isp { get; set; }
        public InterfaceInfo Interface { get; set; }
        public ServerInfo Server { get; set; }
        public ResultInfo Result { get; set; }
    }

    public class PingInfo
    {
        public double Jitter { get; set; }
        public double Latency { get; set; }
        public double Low { get; set; }
        public double High { get; set; }
    }

    public class TransferInfo
    {
        public double Bandwidth { get; set; }
        public double Bytes { get; set; }
        public double Elapsed { get; set; }
        public LatencyInfo Latency { get; set; }
    }

    public class LatencyInfo
    {
        public double Iqm { get; set; }
        public double Low { get; set; }
        public double High { get; set; }
        public double Jitter { get; set; }
    }

    public class InterfaceInfo
    {
        public string InternalIp { get; set; }
        public string Name { get; set; }
        public string MacAddr { get; set; }
        public bool isVpn { get; set; }
        public string ExternalIp { get; set; }
    }

    public class ServerInfo
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public string Ip { get; set; }
    }

    public class ResultInfo
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public bool Persisted { get; set; }
    }
}

