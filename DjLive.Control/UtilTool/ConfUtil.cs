using System;
using System.Collections.Generic;
using System.Text;
using DjLive.Control.Model.ConfModel.LiveService;

namespace DjLive.Control.UtilTool
{
    public class ConfUtil
    {
        private const string Blanking = "     ";
        public static string BuildConfString<T>(T item, string prefix = "")
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var type = typeof(T);
                foreach (var propertyInfo in type.GetProperties())
                {
                    if (string.Equals(propertyInfo.Name, "confTitle", StringComparison.OrdinalIgnoreCase)) continue;
                    dynamic obj = propertyInfo.GetValue(item, null);
                    if (obj == null) continue;

                    if (propertyInfo.PropertyType.IsValueType)
                    {
                        sb.AppendLine($@"{prefix}{propertyInfo.Name}{Blanking}{obj};");
                    }
                    if (propertyInfo.PropertyType == typeof(string) || propertyInfo.PropertyType == typeof(String))
                    {
                        if (string.IsNullOrWhiteSpace(obj)) continue;
                        sb.AppendLine($@"{prefix}{propertyInfo.Name}{Blanking}{obj};");
                    }
                    else if (propertyInfo.PropertyType.IsGenericType)
                    {
                        if (propertyInfo.PropertyType == typeof(Dictionary<string, string>))
                        {
                            sb.AppendLine($@"{prefix}{propertyInfo.Name}{"{"}");
                            foreach (var pair in obj)
                            {
                                sb.AppendLine($@"{prefix}{Blanking}{pair.Key}{Blanking}{pair.Value};");
                            }
                            sb.AppendLine($@"{prefix}{"}"}");
                        }
                        else
                        {
                            foreach (var value in obj)
                            {
                                string title = propertyInfo.Name;
                                string valuePrefix = prefix;
                                if (value is EngineOption)
                                {
                                    title = value.name;
                                    valuePrefix = $@"{prefix}engine ";
                                }
                                if (value is TranscodeOption)
                                {
                                    title = value.filter;
                                    valuePrefix = $@"{prefix}transcode ";
                                }
                                if (value is VHostOption)
                                {
                                    title = value.host;
                                    valuePrefix = $@"{prefix}vhost ";
                                }
                                if (value is HttpOption)
                                {
                                    title = value.host;
                                    valuePrefix = $@"{prefix}http_server ";
                                }

                                sb.AppendLine($@"{valuePrefix}{title} {"{"}");
                                sb.AppendLine(BuildConfString(value, Blanking + prefix));
                                sb.AppendLine($@"{prefix}{"}"}");
                            }
                        }
                    }
                    else if (propertyInfo.PropertyType.IsClass)
                    {
                        string valuePrefix = prefix;
                        sb.AppendLine($@"{valuePrefix}{propertyInfo.Name}{"{"}");
                        sb.AppendLine(BuildConfString(obj, Blanking + prefix));
                        sb.AppendLine($@"{prefix}{"}"}");
                    }

                }
                return sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
          
        }
    }
}