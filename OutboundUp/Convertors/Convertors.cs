namespace OutboundUp.Convertors
{
    public class NumericConvertors
    {
        public static double ConvertBpsToMbps(double bytesPerSecond)
        {
            return Math.Round(bytesPerSecond * 0.000008, 2);
        }
    }
}
