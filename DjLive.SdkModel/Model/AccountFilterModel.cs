using DjLive.SdkModel.Enum;

namespace DjLive.SdkModel.Model
{
    public class AccountFilterModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public AccountRoleType RoleType { get; set; }
        public AccountStatType StatType { get; set; }
    }
}