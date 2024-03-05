using System.Collections.Generic;

namespace PlugRMK.UnityUti
{
    public class LogMonitorRelay
    {
        public class LogData
        {
            public string name;
            public object data;

            public LogData(string name, object data)
            {
                this.name = name;
                this.data = data;
            }
        }

        static LogMonitorRelay instance;
        public static LogMonitorRelay Instance
        {
            get
            {
                if (instance == null)
                    instance = new LogMonitorRelay();
                return instance;
            }
        }
        public static bool HasInstance => instance != null;
        public static LogMonitorRelay TryGetInstance() => HasInstance ? instance : null;

        public List<LogData> dataList = new();
    
        public void AddData(string name, object data)
        {
            var foundData = dataList.Find(x => x.name == name);
            if (foundData == null)
                dataList.Add(new(name, data));
            else if (foundData != null && foundData.data != data)
                foundData.data = data;
        }

        public void RemoveData(string name)
        {
            var foundData = dataList.Find(x=>x.name==name);
            if (foundData != null)
                dataList.Remove(foundData);
        }

    }

}
