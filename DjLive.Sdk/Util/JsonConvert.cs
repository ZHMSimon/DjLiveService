using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DjLive.Sdk.Util
{
    public class JsonConvert
    {
        public static string SerializeObject(object value)
        {
            string jsonValue;
            DataContractJsonSerializer js = new DataContractJsonSerializer(value.GetType());
            using (MemoryStream msObj = new MemoryStream())
            {
                js.WriteObject(msObj, value);
                msObj.Position = 0;
                using (StreamReader sr = new StreamReader(msObj, Encoding.UTF8))
                {
                    jsonValue = sr.ReadToEnd();
                }
            }
            return jsonValue;
        }

        public static object DeserializeObject(string json, Type returnType)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(returnType);
                var model = deseralizer.ReadObject(ms);
                return model;
            }
        }

        public static T DeserializeObject<T>(string json)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(T));
                var model = (T)deseralizer.ReadObject(ms);
                return model;
            }
        }
    }
}