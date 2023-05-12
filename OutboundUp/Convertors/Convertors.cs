namespace OutboundUp.Convertors
{
    public static class NumericConvertors
    {
        public static double ConvertBpsToMbps(double bytesPerSecond)
        {
            return Math.Round(bytesPerSecond * 0.000008, 2);
        }

        public static string CovertDateTimeOffsetToZuluString(this DateTimeOffset dateTimeOffset) => dateTimeOffset.ToString("yyyy-MM-ddTHH:mm:ssZ");
    }
}
