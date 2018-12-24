using System;

namespace DjLive.SdkModel.Enum
{
    [Flags]
    public enum AccountRoleType
    {
        Admin = 4096,
        User = 4096>>1,
    }
}