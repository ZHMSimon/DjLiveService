using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DjLive.SdkModel.Model
{
    [DataContract]
    public class KeyNamePair
    {
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Value { get; set; }

        public KeyNamePair(string key, string name)
        {
            Key = key;
            Value = name;
        }
    }
}
