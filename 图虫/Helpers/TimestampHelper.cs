using System;

namespace 图虫.Helpers
{
    public static class TimestampHelper
    {
        public static string GetTimestamp()
        {
            var time = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            return time.ToString();
        }
    }
}
