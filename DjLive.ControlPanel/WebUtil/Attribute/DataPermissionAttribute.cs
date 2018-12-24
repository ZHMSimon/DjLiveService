using System.Collections.Generic;

namespace DjLive.ControlPanel.WebUtil.Attribute
{
    public class DataPermissionAttribute
    {
        public enum DataPermissionType
        {
            ContainChild,
            OnlyCurrent,
        }
        public DataPermissionType PermissionType { get; set; }
    }
}