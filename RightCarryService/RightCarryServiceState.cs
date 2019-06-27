namespace RightCarryService
{
    public enum RightCarryServiceStateEnum
    {
        OFF,
        FATAL,
        ERROR,
        WARN,
        INFO,
        DEBUG
    }

    public class RightCarryServiceState
    {
        public RightCarryServiceStateEnum State { get; set; }

        public string Message { get; set; }

        public RightCarryServiceErrorCodeEnum ErrorCode { get; set; }
    }
}