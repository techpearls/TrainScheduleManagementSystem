using System.Collections.Generic;
using System.Threading;

namespace Trains.Api.Repository
{
    public class Database : IDatabase
    {
        public Dictionary<string, List<string>> TrainSchedule { get; set; }
        public int[] ScheduleCounts { get; set; }
        private readonly object _lockObj;

        public Database()
        {
            TrainSchedule = new Dictionary<string, List<string>>();
            ScheduleCounts = new int[24 * 60]; // Array to hold data for every minute in a day
            _lockObj = new object();
        }

        public List<string> Get(string key)
        {
            return TrainSchedule.ContainsKey(key) ? TrainSchedule[key] : null;
        }

        public Dictionary<string, List<string>> Keys()
        {
            return TrainSchedule;
        }

        public void Set(string key, List<string> value)
        {
            // Tries to obtain a resource lock and times out in 1 second
            if (Monitor.TryEnter(_lockObj, 1000))
            {
                try
                {
                    TrainSchedule.Add(key, value);
                }
                finally
                {
                    Monitor.Exit(_lockObj);
                }
            }
        }

        public void UpdateScheduleCounts(List<int> indices)
        {
            foreach (var index in indices)
            {
                ScheduleCounts[index]++;
            }
        }

        public int GetNext(int start)
        {
            int current = start + 1;

            while(current != start)
            {
                if (current >= ScheduleCounts.Length)
                    current = 0;

                if (ScheduleCounts[current] >= 2)
                    return current;

                current++;
            }

            return -1;
        }
    }
}
