using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace 图虫.Helpers
{
    public static class TimestampHelper
    {
        public static string GetTimestamp()
        {
            try
            {
                var time = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                return time.ToString();
            }
            catch { return "1970-1-1"; }
        }
    }
}
