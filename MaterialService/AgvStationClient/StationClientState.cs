namespace AgvStationClient
{
    public enum StationClientStateEnum
    {
        INFO,
        ERROR,
        FATAL,
    }

    internal class StationClientState
    {
        public object State { get; set; }
        public string Message { get; set; }
    }
}