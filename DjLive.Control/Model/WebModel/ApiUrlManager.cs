namespace DjLive.Control.Model.WebModel
{
    /// <summary>
    /// 只提供json数据格式接口，要求请求和响应的数据全都是json。
    /// 发生错误时，支持HTTP错误码，或者json中的code错误码。
    /// </summary>
    public class ApiUrlManager
    {
        private static string Host = "";
        private const string server = "4481";	//服务器标识
        private const string versions = "/api/v1/versions";  //获取服务器版本信息
        private const string summaries = "/api/v1/summaries";    //获取服务器的摘要信息
        private const string rusages = "/api/v1/rusages";    //获取服务器资源使用信息
        private const string self_proc_stats = "/api/v1/self_proc_stats";  //获取服务器进程信息
        private const string system_proc_stats = "/api/v1/system_proc_stats";    //获取服务器所有进程情况
        private const string meminfos = "/api/v1/meminfos";  //获取服务器内存使用情况
        private const string authors = "/api/v1/authors";    //获取作者、版权和License信息
        private const string features = "/api/v1/features";  //获取系统支持的功能列表
        private const string requests = "/api/v1/requests";  //获取请求的信息，即当前发起的请求的详细信息
        private const string vhosts = "/api/v1/vhosts";  //获取服务器上的vhosts信息
        private const string streams = "/api/v1/streams";    //获取服务器的streams信息
        private const string clients = "/api/v1/clients";	//获取服务器的clients信息，默认获取前10个
        private const string kickoff = "/api/v1/clients";  //Delete 需要踢掉的Client的ID

        public enum ApiMethod
        {
            Versions,
            Summaries,
            Rusages,
            SelfProcStats,
            SystemProcStats,
            Meminfos,
            Authors,
            Features,
            Requests,
            Vhosts,
            Streams,
            Clients,
            Kickoff
        }

        public static string ApiGetUrl(ApiMethod method, string clientId = "")
        {
            string url = Host + GetRelationUrlByType(method);
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                url += $@"/{clientId}";
            }
            return url;
        }

        private static string GetRelationUrlByType(ApiMethod method)
        {
            string relationUrl = string.Empty;
            switch (method)
            {
                case ApiMethod.Versions:
                {
                    return versions;
                }
                case ApiMethod.Clients:
                {
                    return clients;
                }
                case ApiMethod.Authors:
                {
                    return authors;
                }
                case ApiMethod.Features:
                {
                    return features;
                }
                case ApiMethod.Kickoff:
                {
                    return kickoff;
                }
                case ApiMethod.Meminfos:
                {
                    return meminfos;
                }
                case ApiMethod.Requests:
                {
                    return requests;
                }
                case ApiMethod.Rusages:
                {
                    return rusages;
                }
                case ApiMethod.SelfProcStats:
                {
                    return self_proc_stats;
                }
                case ApiMethod.Streams:
                {
                    return streams;
                }
                case ApiMethod.Summaries:
                {
                    return summaries;
                }
                case ApiMethod.SystemProcStats:
                {
                    return system_proc_stats;
                }
                case ApiMethod.Vhosts:
                {
                    return vhosts;
                }
            }
            return relationUrl;
        }

        public static void Init(string host)
        {
            Host = host;
        }
    }
}