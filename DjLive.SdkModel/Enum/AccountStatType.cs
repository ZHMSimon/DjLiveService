namespace DjLive.SdkModel.Enum
{
    public enum AccountStatType
    {
        Normal = 1,
        Disable = 1 << 1,
        Verify = 1 << 2,
        FreezeReason1 = 1 << 3,
        FreezeReason2 = 1 << 4,
    }
}