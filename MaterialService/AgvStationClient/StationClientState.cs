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
        public DateTime CreateDateTime { get; set; }
    }
}