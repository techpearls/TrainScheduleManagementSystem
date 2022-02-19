using System;
using System.Collections.Generic;
using System.Linq;

namespace Trains.Api.Utilities
{
    public class SanitizedTimes
    {
        public List<string> InvalidTimes { get; set; }
        public List<string> ValidTimes { get; set; }

        public SanitizedTimes(List<string> invalidTimes, List<string> validTimes)
        {
            InvalidTimes = invalidTimes;
            ValidTimes = validTimes;
        }
    }

    public static class DataSanitizerUtility
    {
        public static SanitizedTimes SanitizeTimes(List<string> times)
        {
            List<string> invalidTimes = new();
            List<string> validTimes = new();
            
            foreach (var time in times)
            {
                var hourAndMinute = time.Split(':')
                                    .Where(hm => int.TryParse(hm, out _))
                                    .Select(int.Parse)
                                    .ToList();

                if(hourAndMinute.Count != 2)
                {
                    invalidTimes.Add(time);
                }
                else if(hourAndMinute[0] < 0 || hourAndMinute[0] > 23 || hourAndMinute[1] < 0 || hourAndMinute[1] > 59)
                {
                    invalidTimes.Add(time);
                }
                else
                {
                    validTimes.Add($"{hourAndMinute[0]:00}:{hourAndMinute[1]:00}");
                }                

            }
            return new SanitizedTimes(invalidTimes, validTimes);
        }
    }
}
