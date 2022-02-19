using System;
using System.Collections.Generic;
using Trains.Api.Repository;
using Trains.Api.Utilities;

namespace Trains.Api.Services
{
    public interface IScheduleService
    {
        public List<string> GetSchedule(string id);
        public void CreateSchedule(string id, List<string> times);
        public string GetNext(string time);
        public Dictionary<string, List<string>> GetAll();
    }

    /// <summary>
    /// This class will contain business logic and will interact with the DB layer
    /// </summary>
    public class ScheduleService : IScheduleService
    {
        private readonly IDatabase _db;

        public ScheduleService(IDatabase database = null)
        {
            _db = database ?? new Database();
        }

        public void CreateSchedule(string id, List<string> times)
        {
            if (_db.Get(id) != null)
            {
                throw new ArgumentException($"Schedule already exists for train {id}");
            }

            var sanitizedTimes = DataSanitizerUtility.SanitizeTimes(times);

            if (sanitizedTimes.InvalidTimes.Count > 0)
            {
                throw new ArgumentException($"Please make sure times passed in are valid. Invalid values: {string.Join(',', sanitizedTimes.InvalidTimes)}");
            }

            if (sanitizedTimes.ValidTimes.Count > 0)
            {
                _db.Set(id, sanitizedTimes.ValidTimes);
                var indices = TimeConversionUtility.GetArrayIndices(sanitizedTimes.ValidTimes);
                _db.UpdateScheduleCounts(indices);
            }
        }

        public List<string> GetSchedule(string id)
        {
            return _db.Get(id);
        }

        public string GetNext(string time)
        {
            var sanitizedTimes = DataSanitizerUtility.SanitizeTimes(new List<string> { time });

            if (sanitizedTimes.InvalidTimes.Count > 0)
            {
                throw new ArgumentException("Please make sure time passed in is valid");
            }

            int start = TimeConversionUtility.GetArrayIndex(time);

            int nextIndex = _db.GetNext(start);

            if (nextIndex == -1)
                return string.Empty;

            return TimeConversionUtility.GetTimeFromIndex(nextIndex);
        }

        public Dictionary<string, List<string>> GetAll()
        {
            return _db.Keys();
        }
    }
}
