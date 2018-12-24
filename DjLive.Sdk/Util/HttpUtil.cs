using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using DjLive.SdkModel;

namespace DjLive.Sdk.Util
{
    public class HttpUtil
    {
        public static readonly TimeSpan TimeOutSpan = new TimeSpan(0,1,0);
        public static async Task<string> GetAsync(string url, Dictionary<string, string> headers = null)
        {
            using (HttpClient httpClient = new HttpClient {MaxResponseContentBufferSize = 256000})
            {
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> keyValuePair in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                httpClient.DefaultRequestHeaders.Add("user-agent",
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
                httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                HttpResponseMessage response = await httpClient.GetAsync(url);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    default:
                    {
                        return ParseState2Response(response);
                    }
                }
            }
        }

        public static async Task<string> PostAsync(string url, string jsonString = "",
            Dictionary<string, string> headers = null)
        {
            using (HttpClient httpClient = new HttpClient() {MaxResponseContentBufferSize = 256000,Timeout = TimeOutSpan})
            {
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> keyValuePair in headers)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                var response = await httpClient.PostAsync(url,
                    new StringContent(jsonString, Encoding.UTF8, "application/json"));
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    default:
                    {
                        return ParseState2Response(response);
                    }
                }
            }
        }

        public static async Task<string> PutAsync(string url, string jsonString = "",
            Dictionary<string, string> headers = null)
        {
            using (HttpClient httpClient = new HttpClient {MaxResponseContentBufferSize = 256000})
            {
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> keyValuePair in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                var response = await httpClient.PutAsync(url,
                    new StringContent(jsonString, Encoding.UTF8, "application/json"));
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    default:
                    {
                        return ParseState2Response(response);
                    }
                }
            }
        }

        private static string ParseState2Response(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                {
                    return "{\"ApiCode\":2,\"Message\":\"对象未找到\"}";
                }
                case HttpStatusCode.Conflict:
                {
                    return "{\"ApiCode\":3,\"Message\":\"对象重复\"}";
                }
                case HttpStatusCode.BadRequest:
                {
                    return "{\"ApiCode\":1,\"Message\":\"未知错误\"}";
                }
            }
            return "{\"ApiCode\":1,\"Message\":\"未知错误\"}";
        }

        public static async Task<string> DeleteAsync(string url,  Dictionary<string, string> headers = null)
        {
            using (HttpClient httpClient = new HttpClient {MaxResponseContentBufferSize = 256000})
            {
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> keyValuePair in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                httpClient.DefaultRequestHeaders.Add("user-agent",
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
                httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
                HttpResponseMessage response = await httpClient.DeleteAsync( url);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    default:
                    {
                        return ParseState2Response(response);
                    }
                }
            }
        }

        //获取 callback信息
        public static async Task<CallbackResponseBase> GetCallbackResponseFromStream(Stream stream)
        {
            CallbackResponseBase callbackInfo = null;
            stream.Position = 0;
            using (var sr = new StreamReader(stream))
            {
                var json = await sr.ReadToEndAsync();
                callbackInfo = JsonConvert.DeserializeObject<CallbackResponseBase>(json);
            }
            stream.Close();
            return callbackInfo;
        }


    }
}