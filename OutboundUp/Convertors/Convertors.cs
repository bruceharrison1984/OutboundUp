namespace OutboundUp.Convertors
{
    public static class NumericConvertors
    {
        public static double ConvertBpsToMbps(double bytesPerSecond) => Math.Round(bytesPerSecond * 0.000008, 2);
    }
}
