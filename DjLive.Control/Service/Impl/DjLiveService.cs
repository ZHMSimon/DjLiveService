using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DjLive.Control.Model.ConfModel.LiveService;
using DjLive.Control.Model.WebModel;
using DjLive.Control.Model.WebModel.Data;
using DjLive.Control.UtilTool;
using DjUtil.Tools.RetryPolicy;
using Newtonsoft.Json;
using Renci.SshNet;
using DjUtil.Tools;

namespace DjLive.Control.Service.Impl
{
    public class DjLiveService : IDjLiveInterface
    {
        private string _host = "47.104.69.60";
        private string _hostUrl = "47.104.69.60";
        private string _apiUrl = "47.104.69.60:1985";
        private string _username = "root";
        private string _password = "1qaz@WSX!**$@(#";
        private int _port = 22;
        private int _apiPort = 1985;
        private int _httpPort = 8080;
        private int rtmpPort = 1935;

        private RetryPolicy requestRetryPolicy = new RetryPolicy(new CatchAllRetryDetectionStrategy(), new DefaultRetryStrategy());

        public object UtilTools { get; private set; }

        public DjLiveService()
        {
            var reader = new System.Configuration.AppSettingsReader();
            _host  = (string)reader.GetValue("host", typeof(string));
            _hostUrl = (string)reader.GetValue("hostUrl", typeof(string));
            _apiUrl = (string)reader.GetValue("apiUrl", typeof(string));
            _username = (string)reader.GetValue("userName", typeof(string));
            _password = (string)reader.GetValue("password", typeof(string));
            _port = (int)reader.GetValue("port", typeof(int));
            _apiUrl = string.IsNullOrEmpty(_apiUrl)?_hostUrl + $":{_apiPort}" : _apiUrl;
            ApiUrlManager.Init(_apiUrl);
        }

        public DjLiveService(LiveServiceConfig config)
        {
            this._host = config.Host;
            this._hostUrl = config.HostUrl;
            this._username = config.UserName;
            this._password = config.Password;
            _port = config.SshPort;
            _apiPort = config.ApiPort;
            _httpPort = config.HttpPort;
            rtmpPort = config.RtmpPort;
            _apiUrl = _hostUrl + $":{_apiPort}";
            ApiUrlManager.Init(_apiUrl);

        }
        public DjLiveService(string host,string hostUrl,string userName,string password,int port = 22)
        {
            this._host = host;
            this._hostUrl = hostUrl;
            this._username = userName;
            this._password = password;
            _port = port;
            _apiUrl = hostUrl + ":1985";
            ApiUrlManager.Init(_apiUrl);

        }
        /// <summary>
        /// 控制台控制linux
        /// </summary>
        /// <param name="client"></param>
        /// <param name="commandStr"></param>
        /// <param name="successCallback"></param>
        /// <param name="errorCallback"></param>
        private void ExcuteCommand(SshClient client, string commandStr, Action<string> successCallback, Action<string> errorCallback)
        {
            try
            {
                var inStream = new Renci.SshNet.Common.PipeStream();
                var outStream = new MemoryStream();
                var exStream = new MemoryStream();
                var shell = client.CreateShell(inStream, outStream, exStream);
                var streamWriter = new StreamWriter(inStream) { AutoFlush = true };
                shell.Started += (sender, args) =>
                {
                    while (true)
                    {
                        var command = Console.ReadLine();
                        streamWriter.WriteLine(command);
                        streamWriter.Flush();
                    }

                };
                int outIndex = 0;
                var td = new Thread(() =>
                {
                    while (true)
                    {
                        var length = outStream.Length == 0 ? 0 : outStream.Length;
                        if (length - outIndex == 0) continue;
                        byte[] buffer = new byte[length - outIndex];
                        outStream.Position = outIndex;
                        outStream.Read(buffer, 0, buffer.Length);
                        outIndex = (int)length;
                        Console.WriteLine(Encoding.UTF8.GetString(buffer));
                        Thread.Sleep(1000);
                    }

                });
                td.Start();
                shell.Start();




                //client.ErrorOccurred += (sender, eventArgs) =>
                //{
                //    Console.WriteLine(eventArgs.Exception);
                //};
                //client.HostKeyReceived += (sender, eventArgs) =>
                //{
                //    Console.WriteLine(eventArgs.HostKeyName);
                //};

                //var command = client.CreateCommand(commandStr.ToString());
                //command.CommandTimeout = new TimeSpan(0, 0, 10);
                //command.Execute(commandStr);


                //if (!string.IsNullOrWhiteSpace(command.Error))
                //{
                //    errorCallback(command.Error);
                //    //result.Append(command.Error + "\r\n");
                //}
                //else
                //{
                //    successCallback(command.Result);
                //}
            }
            catch (Exception e1)
            {
                errorCallback(e1.Message);
            }
        }
        public void InstallLiveService(Action<int, string> resultCallback,bool repair)
        {
            if (IsInstalledService() && !repair) return;
            using (SshClient client = new SshClient(_host,_port, _username, _password))
            {
                client.Connect();
                StringBuilder installLogBuilder = new StringBuilder();
                try
                {
                    var inStream = new Renci.SshNet.Common.PipeStream();
                    var outStream = new MemoryStream();
                    var exStream = new MemoryStream();
                    var shell = client.CreateShell(inStream, outStream, exStream);
                    var streamWriter = new StreamWriter(inStream) { AutoFlush = true };
                    var daemonThread = new Thread(() =>
                    {
                        int outIndex = 0;
                        while (true)
                        {
                            Thread.Sleep(200);
                            var length = outStream.Length == 0 ? 0 : outStream.Length;
                            if (length - outIndex == 0) continue;
                            byte[] buffer = new byte[length - outIndex];
                            outStream.Position = outIndex;
                            outStream.Read(buffer, 0, buffer.Length);
                            outIndex = (int)length;
                            var result = Encoding.UTF8.GetString(buffer);
                            installLogBuilder.AppendLine(result);
                            Console.WriteLine(result);
                            if (result.Contains("Live Stream Service Installed!"))
                            {
                                shell.Stop();
                                shell.Dispose();
                                break;
                            }

                        }
                        resultCallback.Invoke(0, "Install Success.");
                        Thread.CurrentThread.Abort();
                    });
                    shell.Starting += (state, arg) =>
                    {

                    };
                    shell.ErrorOccurred += (state, arg) =>
                    {

                    };
                    shell.Stopped += (state, arg) =>
                    {

                    };
                    shell.Stopping+= (state, arg) =>
                    {

                    };
                    shell.Started += (sender, args) =>
                    {
                        daemonThread.Start();
                        Thread.Sleep(5000);
                        var sb = new StringBuilder();
                        sb.AppendLine(@"echo Start Stream Service Installed!");
                        sb.AppendLine("yum install -y autoconf automake cmake freetype-devel gcc gcc-c++ git libtool make mercurial nasm pkgconfig zlib - devel");
                        sb.AppendLine("yum update -y nss curl libcurl");
                        sb.AppendLine("git clone https://github.com/ossrs/srs");
                        sb.AppendLine("cd ~/srs/trunk");
                        sb.AppendLine("./configure --full&& make");
                        sb.AppendLine(@"echo Live Stream Service Installed!");
                        streamWriter.WriteLine(sb);
                        streamWriter.Flush();
                        Thread.CurrentThread.Join();
                    };
                    shell.Start();
                }
                catch (Exception e)
                {
                    resultCallback(-1, e.Message);
                    client.Disconnect();
                    client.Dispose();
                    throw;
                }
                finally
                {
                    //todo: save installLogBuilder

                }
            }
        }
        private bool IsInstalledService()
        {
            try
            {
                bool result = false;
                string remoteFileName = $@"/root/srs/trunk/conf/srs.conf";
                using (var sftp = new SftpClient(_host, _port, _username, _password))
                {
                    sftp.Connect();
                    if (sftp.Exists(remoteFileName))
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public string UploadLogoFile(string logoName,string filePath)
        {
            try
            {
                string remoteFileName = $@"/root/srs/trunk/etc/{logoName}.png";
                using (var sftp = new SftpClient(_host, _port, _username, _password))
                {
                    sftp.Connect();
                    if (sftp.Exists(remoteFileName))
                    {
                        sftp.DeleteFile(remoteFileName);
                    }
                    
                    using (var rfs = sftp.OpenWrite(remoteFileName))
                    {
                        int offsetPos = 0;
                        int maxBufferSize = 1024 * 50;
                        byte[] buffer;
                        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            fs.Position = 0;
                            while (fs.Position < fs.Length)
                            {
                                int bufferLength = (fs.Length - offsetPos) > maxBufferSize ? maxBufferSize : (int)(fs.Length - offsetPos);
                                buffer = new byte[bufferLength];
                                fs.Read(buffer, 0, (int)bufferLength);
                                offsetPos += bufferLength;
                                rfs.Write(buffer, 0, bufferLength);
                                if (bufferLength < maxBufferSize) rfs.Flush();
                            }
                           
                        }
                    }
                }
                return remoteFileName;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public string RemoveLogoFile(string logoName)
        {
            try
            {
                string remoteFileName = $@"/root/srs/trunk/etc/{logoName}.png";
                using (var sftp = new SftpClient(_host, _port, _username, _password))
                {
                    sftp.Connect();
                    if (sftp.Exists(remoteFileName))
                    {
                        sftp.DeleteFile(remoteFileName);
                    }
                }
                return remoteFileName;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public string SetLiveServiceConf(HostOption option, string confName)
        {
            try
            {
                string remoteFileName = $@"/root/srs/trunk/conf/{confName}.conf";
                using (var sftp = new SftpClient(_host,_port, _username, _password))
                {
                    sftp.Connect();
                    if (sftp.Exists(remoteFileName))
                    {
                        sftp.DeleteFile(remoteFileName);
                    }
                    //var configText = File.ReadAllText(@"C:\Users\Meng Zhang\Desktop\test.conf");
                    var configText = ConfUtil.BuildConfString(option);
                    sftp.WriteAllText(remoteFileName, configText);
                }
                ReloadHotService(confName, ((i, s) => { }));
                return remoteFileName;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        public HostOption GetLiveServiceConf(string confName)
        {
            try
            {
                string remoteFileName = $@"/root/srs/trunk/conf/{confName}.conf";
                string remoteJsonName = $@"/root/srs/trunk/conf/{confName}.json";
                HostOption result = null;
                using (var sftp = new SftpClient(_host,_username, _password))
                {
                    sftp.Connect();
                    if (sftp.Exists(remoteFileName))
                    {
                        var json = sftp.ReadAllText(remoteJsonName);
                        result = JsonConvert.DeserializeObject<HostOption>(json);
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void StartLiveService(string confName, Action<int, string> resultCallback)
        {
            using (SshClient client = new SshClient(_host,_port, _username, _password))
            {
                try
                {
                    client.Connect();
                    var command = client.CreateCommand($@"~/srs/trunk/objs/srs -c ~/srs/trunk/conf/{confName}.conf;");
                    command.Execute();
                    if (!string.IsNullOrWhiteSpace(command.Error))
                    {
                        resultCallback(-1, command.Error);
                    }
                    else
                    {
                        resultCallback(0, command.Result);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
                finally
                {
                    client.Disconnect();
                    client.Dispose();
                }

            }
        }

        public void StopLiveService()
        {
            //todo:暂无
            throw new NotImplementedException();
        }

        public void ReloadHotService(string confName, Action<int, string> resultCallback)
        {
            using (SshClient client = new SshClient(_host,_port, _username, _password))
            {
                try
                {
                    client.Connect();
                    var command = client.CreateCommand($@"killall -1 srs");
                    command.Execute();
                    if (!string.IsNullOrWhiteSpace(command.Error))
                    {
                        resultCallback(-1, command.Error);
                    }
                    else
                    {
                        resultCallback(0, command.Result);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
                finally
                {
                    client.Disconnect();
                    client.Dispose();
                }

            }
        }

        public async Task<VersionRoot> GetVersionInfo()
        {
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.Versions);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await HttpUtil.GetAsync(url);
            });
            try
            {
                return JsonConvert.DeserializeObject<VersionRoot>(strResult);
            }
            catch (Exception e)
            {
                //todo: log
                return null;
            }
        }

        public async Task<SummaryRoot> GetSummaryInfo()
        {
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.Summaries);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await HttpUtil.GetAsync(url);
            });
            try
            {
                return JsonConvert.DeserializeObject<SummaryRoot>(strResult);
            }
            catch (Exception e)
            {
                //todo: log
                return null;
            }
        }

        public async Task<ResourceUsedRoot> GetResourceUsedInfo()
        {
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.Rusages);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await HttpUtil.GetAsync(url);
            });
            try
            {
                return JsonConvert.DeserializeObject<ResourceUsedRoot>(strResult);
            }
            catch (Exception e)
            {
                //todo: log
                return null;
            }
        }

        public async Task<SystemProcStatsRoot> GetSystemProcStatsInfo()
        {
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.SystemProcStats);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await  HttpUtil.GetAsync(url);
            });
            try
            {
                return JsonConvert.DeserializeObject<SystemProcStatsRoot>(strResult);
            }
            catch (Exception e)
            {
                //todo: log
                return null;
            }
        }

        public async Task<SelfProcessStatRoot> GetSelfProcessStatInfo()
        {
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.SelfProcStats);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await HttpUtil.GetAsync(url);
            });
            try
            {
                return JsonConvert.DeserializeObject<SelfProcessStatRoot>(strResult);
            }
            catch (Exception e)
            {
                //todo: log
                return null;
            }
        }

        public async Task<MemRoot> GetMemInfo()
        {
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.Meminfos);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await HttpUtil.GetAsync(url);
            });
            try
            {
                return JsonConvert.DeserializeObject<MemRoot>(strResult);
            }
            catch (Exception e)
            {
                //todo: log
                return null;
            }
        }

        public async Task<AuthorRoot> GetAuthorInfo()
        {
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.Authors);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await HttpUtil.GetAsync(url);
            });
            try
            {
                return JsonConvert.DeserializeObject<AuthorRoot>(strResult);
            }
            catch (Exception e)
            {
                //todo: log
                return null;
            }
        }

        public async Task<FeatherRoot> GetFeatherInfo()
        {
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.Features);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await HttpUtil.GetAsync(url);
            });
            try
            {
                return JsonConvert.DeserializeObject<FeatherRoot>(strResult);
            }
            catch (Exception e)
            {
                //todo: log
                return null;
            }
        }

        public async Task<VhostRoot> GetVhostInfo()
        {
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.Vhosts);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await HttpUtil.GetAsync(url);
            });
            try
            {
                return JsonConvert.DeserializeObject<VhostRoot>(strResult);
            }
            catch (Exception e)
            {
                //todo: log
                return null;
            }
        }

        public async Task<StreamRoot> GetStreamInfo()
        {
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.Streams);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await HttpUtil.GetAsync(url);
            });
            try
            {
                return JsonConvert.DeserializeObject<StreamRoot>(strResult);
            }
            catch (Exception e)
            {
                //todo: log
                return null;
            }
        }

        public async Task<ClientRoot> GetClientInfo()
        {
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.Clients);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await HttpUtil.GetAsync(url);
            });
            try
            {
                return JsonConvert.DeserializeObject<ClientRoot>(strResult);
            }
            catch (Exception e)
            {
                //todo: log
                return null;
            }
        }

        public async Task DisConnectClientById(string id)
        {
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.Kickoff, id);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await HttpUtil.GetAsync(url);
            });
        }

        public async Task DisConnectClientByApp(string app)
        {
            throw new NotImplementedException();
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.Kickoff, app);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await HttpUtil.GetAsync(url);
            });
        }

        public async Task DisConnectClientByStream(string app, string stream)
        {
            throw new NotImplementedException();
            var url = ApiUrlManager.ApiGetUrl(ApiUrlManager.ApiMethod.Kickoff, app + "/" + stream);
            var strResult = string.Empty;
            await requestRetryPolicy.ExecuteAction(async () =>
            {
                strResult = await HttpUtil.GetAsync(url);
            });
        }
    }
}