using System;

namespace AgvStationClient
{
    public enum StationClientStateEnum
    {
        INFO,
        ERROR,
        FATAL,
    }

    public class StationClientState
    {
        public StationClientStateEnum State { get; set; }
        public string Message { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"【{State}】:{Message} {CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss")}";
        }
    }
}