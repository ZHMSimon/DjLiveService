using System.Collections.Generic;

namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class SecurityOption
    {
        public string enabled { get; set; } = "off";
        public List<SecuritySetting> settings { get; set; }

    }
}