namespace RightCarryService
{
    public enum RightCarryServiceErrorCodeEnum
    {
        NORMAL=0,
        CARRYIN_INITIAL=1,
        CARRYIN_PRODUCT =2,
        CARRYIN_MATERIAL = 3,
        CARRYIN_INOUT=4,
        CARRYIN_REQ=5,
        CARRYIN_RESET=6,
        CARRYIN_REREQ=7,
        CARRYIN_REFIN=8,
        CARRYOUT_INITIAL = 51,
        CARRYOUT_PRODUCT = 52,
        CARRYOUT_MATERIAL = 53,
        CARRYOUT_INOUT = 54,
        CARRYOUT_REQ = 55,
        CARRYOUT_RESET = 56,
        CARRYOUT_REREQ = 57,
        CARRYOUT_REFIN = 58
    }
}