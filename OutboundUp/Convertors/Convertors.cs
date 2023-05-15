using System.Text.Json;

namespace OutboundUp.Convertors
{
    public static class NumericConvertors
    {
        public static double ConvertBpsToMbps(double bytesPerSecond) => Math.Round(bytesPerSecond * 0.000008, 2);
    }

    public static class JSONConvertors
    {
        public static bool IsStringValidJson(string json)
        {
            if (json == null)
                return false;

            try
            {
                JsonDocument.Parse(json);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}
