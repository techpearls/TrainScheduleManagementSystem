using System;
using System.Collections.Generic;

namespace Trains.Api.Repository
{
    public interface IDatabase
    {
        public void Set(string key, List<string> value);
        public List<string> Get(string key);
        public Dictionary<string, List<string>> Keys();
        void UpdateScheduleCounts(List<int> indices);
        int GetNext(int start);
    }
}
