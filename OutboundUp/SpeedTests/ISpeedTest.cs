namespace OutboundUp.SpeedTests
{
    public interface IOoklaSpeedTest
    {
        Task<SpeedTestResult> RunSpeedTest();
    }
}