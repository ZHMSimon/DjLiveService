using DjLive.SdkModel.Enum;

namespace DjLive.ControlPanel.WebUtil.Attribute
{
    
    public class RolePermissionAttribute:System.Attribute
    {
        public enum RolePermissionType
        {
            Eq,
            Gt,
        }
        public RolePermissionType PermissionType { get; set; }
        public AccountRoleType PermissionRoleType { get; set; }
    }
}