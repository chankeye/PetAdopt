using System;

namespace PetAdopt.Utilities
{
    public class TransformTime
    {
        public static DateTime UtcToLocalTime(DateTime utcTime)
        {
            return DateTime.SpecifyKind(utcTime, DateTimeKind.Utc).ToLocalTime();
        }
    }
}