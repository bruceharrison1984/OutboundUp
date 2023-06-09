﻿using OutboundUp.Convertors;
using OutboundUp.Models;
using System.Diagnostics;
using System.Text.Json;

namespace OutboundUp.SpeedTests.Ookla
{
    public class OoklaSpeedTest : IOoklaSpeedTest
    {
        private readonly ILogger<OoklaSpeedTest> _logger;
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
        };

        public OoklaSpeedTest(ILogger<OoklaSpeedTest> logger)
        {
            _logger = logger;
        }

        public async Task<SpeedTestResult> RunSpeedTest()
        {
            _logger.LogInformation("Starting Ookla Speed Test");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/tools/speedtest",
                    Arguments = "--accept-license -f json -u Mbps",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };
            process.EnableRaisingEvents = true;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();

            if (!JSONConvertors.IsStringValidJson(output))
                throw new Exception("Ookla results is not valid JSON. An error likely occured during the speed test to cause this error.");


            var ooklaResult = JsonSerializer.Deserialize<OoklaTestResults>(output, options);

            var result = new SpeedTestResult
            {
                UnixTimestampMs = ooklaResult!.Timestamp.ToUnixTimeMilliseconds(),
                IsSuccess = true,
                ServerHostName = ooklaResult.Server.Host,
                DownloadSpeed = NumericConvertors.ConvertBpsToMbps(ooklaResult.Download.Bandwidth),
                UploadSpeed = NumericConvertors.ConvertBpsToMbps(ooklaResult.Upload.Bandwidth),
                PingAverage = ooklaResult.Ping.Latency,
                PingHigh = ooklaResult.Ping.High,
                PingLow = ooklaResult.Ping.Low,
                DownloadLatencyAverage = ooklaResult.Download.Latency.Iqm,
                DownloadLatencyHigh = ooklaResult.Download.Latency.High,
                DownloadLatencyLow = ooklaResult.Download.Latency.Low,
                UploadLatencyAverage = ooklaResult.Upload.Latency.Iqm,
                UploadLatencyHigh = ooklaResult.Upload.Latency.High,
                UploadLatencyLow = ooklaResult.Upload.Latency.Low,
            };

            return result;
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new Exception("Error occured while running Ookla speed test!");
        }
    }
}
