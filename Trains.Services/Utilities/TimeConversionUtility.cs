using System;
using System.Collections.Generic;
using System.Linq;

namespace Trains.Api.Utilities
{
    public static class TimeConversionUtility
    {
        public static List<int> GetArrayIndices(List<string> times)
        {
            List<int> indices = new List<int>();

            foreach (var time in times)
            {
                indices.Add(GetArrayIndex(time));
            }

            return indices;
        }

        public static int GetArrayIndex(string time)
        {
            var hourAndMinute = GetHourAndMinute(time);
            return hourAndMinute[0] * 60 + hourAndMinute[1];
        }

        public static List<int> GetHourAndMinute(string time)
        {
            var hourAndMinute = time.Split(':')
                                    .Select(int.Parse)
                                    .ToList();
            return hourAndMinute;
        }

        public static string GetTimeFromIndex(int index)
        {
            int hour = index / 60;
            int minute = index % 60;
            return $"{hour:00}:{minute:00}";
        }
    }
}
