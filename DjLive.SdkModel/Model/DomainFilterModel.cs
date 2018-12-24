using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DjLive.SdkModel.Enum;

namespace DjLive.SdkModel.Model
{
    public class DomainFilterModel
    {
        public string SourceDomain { get; set; }
        public SourceStreamType SourceType { get; set; }
        public StateType State { get; set; }
    }
}
