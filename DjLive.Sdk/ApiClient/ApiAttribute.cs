using System;

namespace DjLive.Sdk.ApiClient
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ApiAttribute : Attribute
    {
        public string Url { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public ContentType ContentType { get; set; }

        public static string ParseContentType(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Json:
                {
                    return "application/json";
                }
                case ContentType.Text:
                {
                    return "text/plain";
                    }
                case ContentType.Xml:
                {
                    return "text/xml";
                }
                case ContentType.Html:
                {
                    return "text/html";
                }
                default:
                        return "application/json";
            }
        }
    }
}