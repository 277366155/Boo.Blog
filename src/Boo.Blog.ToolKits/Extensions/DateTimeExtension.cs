﻿using System;

namespace Boo.Blog.ToolKits.Extensions
{
    public static class DateTimeExtension
    {
        public static readonly DateTime TimeEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Add(TimeZoneInfo.Local.GetUtcOffset(DateTime.Now));
        public static readonly DateTime TimeUtcEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static long ToTicks(this DateTime dateTime)
        {
            return (long)(dateTime - TimeEpoch).TotalSeconds;
        }
        public static long ToMilliTicks(this DateTime dateTime)
        {
            return (long)(dateTime - TimeEpoch).TotalMilliseconds;
        }

    }
}
