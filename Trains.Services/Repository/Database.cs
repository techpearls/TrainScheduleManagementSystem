using System.Collections.Generic;
using System.Threading;

namespace Trains.Api.Repository
{
    public class Database : IDatabase
    {
        public static Dictionary<string, List<string>> TrainSchedule { get; set; }
        public static int[] ScheduleCounts { get; set; }
        private readonly object _setLock, _updateLock;

        static Database()
        {
            TrainSchedule = new Dictionary<string, List<string>>();
            ScheduleCounts = new int[24 * 60]; // Array to hold data for every minute in a day
        }

        public Database()
        {
            _setLock = new object();
            _updateLock = new object();
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
            if (Monitor.TryEnter(_setLock, 1000))
            {
                try
                {
                    TrainSchedule.Add(key, value);
                }
                finally
                {
                    Monitor.Exit(_setLock);
                }
            }
        }

        public void UpdateScheduleCounts(List<int> indices)
        {
            if(Monitor.TryEnter(_updateLock, 2000))
            {
                try
                {
                    foreach (var index in indices)
                    {
                        ScheduleCounts[index]++;
                    }
                }
                finally
                {
                    Monitor.Exit(_updateLock);
                }
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
