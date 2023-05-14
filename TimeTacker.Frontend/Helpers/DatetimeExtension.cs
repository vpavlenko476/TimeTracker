using System;

namespace TimeTacker.Frontend.Helpers
{
    public static class DatetimeExtension
    {
        public static DateTimeOffset ToUtcDateTimeOffset(this DateTime utcDate)
        {
            return new DateTimeOffset(DateTime.SpecifyKind(utcDate, DateTimeKind.Utc), TimeSpan.Zero);
        }
    }
}