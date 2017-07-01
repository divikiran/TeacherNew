using System;

namespace NPAInspectionWriter.iOS.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime SubtractMonths( this DateTime dt, int months ) => dt.AddMonths( months * -1 );

        public static DateTime SubtractDays( this DateTime dt, double days ) => dt.AddDays( days * -1 );

        public static DateTime SubtractHours( this DateTime dt, double hours ) => dt.AddHours( hours * -1 );

        public static DateTime SubtractMinutes( this DateTime dt, double minutes ) => dt.AddMinutes( minutes * -1 );
    }
}
