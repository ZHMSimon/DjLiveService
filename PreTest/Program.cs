using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using DjLive.Control;
using DjLive.Control.Service.Impl;
using DjLive.Control.UtilTool;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Impl;
using DjLive.CPDao.Interface;
using DjLive.CPService.Impl;
using DjLive.CPService.Interface;
using DjLive.Sdk;
using DjLive.Sdk.Model;
using DjLive.Sdk.Util;
using DjLive.SdkModel;
using DjLive.SdkModel.Enum;
using DjLive.SdkModel.Model;
using DjUtil.Tools;
using DjUtil.Tools.Cryptography;

namespace PreTest
{
    class Program
    {
        private static bool TryVertifyTokenAuth(string authString, out string roomNum, out string userId, out int action)
        {
            
            DateTime ccc = new DateTime(636641535050000000);
            var a = 1.5f /0.2f;
            roomNum = string.Empty;
            userId = string.Empty;
            action = 0;
            var expireTick = DateTime.MinValue.Ticks;
            var token = string.Empty;
            var pairs = authString.Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var pair in pairs)
            {
                var values = pair.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 2)
                {
                    switch (values[0])
                    {
                        case "room":
                            roomNum = values[1];
                            break;
                        case "audienceId":
                            userId = values[1];
                            action = 2;
                            break;
                        case "publisher":
                            userId = values[1];
                            action = 1;
                            break;
                        case "expireTime":
                            long.TryParse(values[1], out expireTick);
                            break;
                        case "token":
                            token = values[1];
                            break;
                    }
                }
                else
                {
                    return false;
                }
            }
            if (new DateTime(expireTick) < DateTime.Now) return false;
            string tokenInfo;
            switch (action)
            {
                case 1:
                    tokenInfo = $"room={roomNum}&publisher={userId}&expireTime={expireTick}";
                    break;
                case 2:
                    tokenInfo = $"room={roomNum}&audienceId={userId}&expireTime={expireTick}";
                    break;
                default:
                    return false;
            }
            var compareToken = EncryptUtils.MD5Encrypt(tokenInfo + $"|76eb7d3b40b64370939e02f04ad6b3a8");
            return string.CompareOrdinal(token, compareToken) == 0;
        }

        public static dynamic String2Image(StringBuilder sb,int width, Stream imgStream,Brush brush, Font font = null, PointF offset = new PointF())
        {
            string str = sb.ToString();
            Graphics g = Graphics.FromImage(new Bitmap(1, 1));
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            if (font == null) font = new Font("微软雅黑", 9);
            //定义范围
            RectangleF desRectangle = new RectangleF();
            desRectangle.Location = offset;
            var height = (int)g.MeasureString(str, font, width, StringFormat.GenericTypographic).Height;
            desRectangle.Size = new SizeF(width, height);

            //水印
            //RectangleF waterRectangle = new RectangleF();
            //waterRectangle.X = 263;
            //waterRectangle.Y = height / 3;
            //waterRectangle.Width = 790;
            //waterRectangle.Height = height / 3;

            ////绘制文字
            //Font font1 = new Font("微软雅黑", 72);
            //g.RotateTransform(5);
            //g.DrawString("测试水印", font1, new SolidBrush(Color.FromArgb(80, Color.Red)), waterRectangle);



            var addimg = new Bitmap(@"d:/img.jpg");
            Bitmap img = new Bitmap(width, height + addimg.Height + 20);

            g = Graphics.FromImage(img);
            g.DrawString(str, font, brush, desRectangle);
            g.DrawImageUnscaled(addimg, width/2 - addimg.Width/2, (int)desRectangle.Height);
            g.DrawString("测试图片",font,Brushes.BurlyWood, width/2 - addimg.Width/2, (int)desRectangle.Height + addimg.Height);
            img.Save(imgStream,ImageFormat.Png);
            imgStream.Close();
            font.Dispose();
            return null;
        }
        private static string CreateTokenAuth(string userId, string roomName)
        {
            var tokenInfo = $"room={roomName}&audienceId={userId}&expireTime={DateTime.Now.AddHours(2).Ticks}";
            var token = EncryptUtils.MD5Encrypt(tokenInfo + $"|76eb7d3b40b64370939e02f04ad6b3a8");
            tokenInfo += $"&token={token}";
            return tokenInfo;
        }
        private static string CreatePublishTokenAuth(string userId, string roomNum)
        {
            var tokenInfo = $"room={roomNum}&publisher={userId}&expireTime={new DateTime(2200, 5, 17).Ticks}";
            var token = EncryptUtils.MD5Encrypt(tokenInfo + $"|76eb7d3b40b64370939e02f04ad6b3a8");
            tokenInfo += $"&token={token}";
            return tokenInfo;
        }
        static void Main(string[] args)
        {
            var a = CreatePublishTokenAuth("anonymous", "anonymousRoom");
            string roomNum;
            string userId;
            int action;
            TryVertifyTokenAuth(a, out roomNum, out userId, out action);
            Console.ReadLine();
            //var callbackInfo =
            //    "rtmp://rtmp.520zhonghua.com/live/?room=201711309601022&publisher=2ddb4253-a838-415a-8ca0-dfd1565943ff&expireTime=694054656000000000&token=aee1ecd9dadf95adac4ca30e10adae94";
            //var pairs = callbackInfo.Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //string userId, roomnum;
            //int action;
            //TryVertifyTokenAuth(pairs[1], out userId,out roomnum,out action);
            //string host = "172.28.10.136";
            //string hostUrl = "172.28.10.136";
            //string username = "root";
            //string password = "P@96*sword";

            //var dj = new DjLiveService(host, hostUrl, username, password,25512);
            //Test0427:
            //dj.UploadLogoFile("testLogo", "d:/xxxx.png");
            //Console.WriteLine("-------------------------------");

            //Console.ReadLine();
            //goto Test0427;
            //return;


            //var xxxx = JsonConvert.DeserializeObject<CallbackResponseBase>("{\"action\": \"on_connect\",\"client_id\": 1985,\"ip\": \"192.168.1.10\",\"vhost\": \"video.test.com\",\"app\": \"live\",\"tcUrl\": \"rtmp://x/x?key=xxx\",\"pageUrl\": \"http://x/x.html\"}");


            TokenManager.GetInstance().InitServicePoint("http://172.28.10.137:62000/", HttpScheme.Http);

            var xxx = TokenManager.GetInstance().UpdateToken("admin", "admin").Result;

            PreTest:
            ApiManager.GetInstance().StateService.ReloadSrsConf("fdd5659abd28416d91b7711581cfa6a0");
            //var delete = ApiManager.GetInstance().StateService.CreateDefaultDomain(new DomainModel()
            //{
            //    SourceDomain = "www.huiguyuedu.com",
            //    Description = "慧谷阅读直播节点",
            //    SourceType = SourceStreamType.Rtmp_Push,
            //    StateType = StateType.Open,
            //    ServerPair = new KeyNamePair("fdd5659abd28416d91b7711581cfa6a0",""),
            //    SecurePolicyPair = new KeyNamePair("76eb7d3b40b64370939e02f04ad6b3a8", ""),
            //});
            Console.WriteLine();
            Console.ReadLine();
            goto PreTest;
            return;
            IStateServiceInterface s = new StateServiceImpl();
            var base64 =  DjUtil.Tools.Cryptography.EncryptUtils.Img2Base64String(@"D:\xxxx.png");
            ILogoTemplateDaoInterface logoTemplateDao = new LogoTemplateDaoImpl();
            IAccountDaoInterface accountDao = new AccountDaoImpl();
            var id = Guid.NewGuid().Str();
            accountDao.AddLogoTemplate("a6697afd9d4642c0b47982e89b53867b", new LogoTemplateEntity()
            {
                Id = id,
                FilePath = "xxx",
                Name = "xxxx",
                Base64Vale = "xxx",
                Option = "xxx",
            });

            return;
            ISecurePolicyDaoInterface securedao = new SecurePolicyDaoImpl();
            IVhostServiceInterface VhostService = new VhostServiceImpl();
            IServerDaoInterface serverDao = new ServerDaoImpl();
            ITranscodeTemplateDaoInterface transcodeTemplateDao = new TranscodeTemplateDaoImpl();
            transcodeTemplateDao.Add2Account("ea778e85cada4de79701062e9c473c4b", new TranscodeTemplateEntity()
            {
                Id = Guid.NewGuid().Str(),
                Name = "TestTranscode",
                VideoOption = "{}",
                AudioOption = "{}"
            });

           
            Console.ReadKey();
            //string host = "172.28.10.136";
            //string hostUrl = "172.28.10.136";
            //string username = "root";
            //string password = "P@96*sword";

            //var dj = new DjLiveService("172.28.59.189", "172.28.59.189","root","1qaz2wsxE");
            //dj.InstallLiveService((code, msg) =>
            //{

            //});

            // var a = UtilTools.BuildConfString(new HostOption()
            //{
            //    http_api =  new ApiOption(""),
            //    stats = new StatOption(),
            //    http_server = new List<HttpOption>()
            //    {
            //        new HttpOption("")
            //    },
            //    VHostOptions = new List<VHostOption>() { new VHostOption("")
            //    {
            //        TranscodeOptions = new List<TranscodeOption>()
            //        {
            //            new TranscodeOption()
            //            {
            //                EngineOptions = new List<EngineOption>()
            //                {
            //                    EngineOption.HdEngineOption(),
            //                    EngineOption.FastEngineOption(),
            //                    EngineOption.SdEngineOption(),
            //                }
            //            }
            //        }
            //    } },
            //});

            //var result = HttpUntil.GetAsync("http://172.28.59.189:1985/api/v1/summaries");
            //Console.WriteLine(result);
            //StringBuilder result = new StringBuilder();
            //StringBuilder commandStr = new StringBuilder();
            //commandStr.Append("/usr/local/nginx/sbin/nginx -s reload  ");
            //using (SshClient client = new SshClient("172.28.59.189", username, "1qaz2wsxE"))
            //{
            //    try
            //    {
            //        client.ErrorOccurred += (sender, eventArgs) =>
            //        {
            //            Console.WriteLine(eventArgs.Exception);
            //        };
            //        client.HostKeyReceived += (sender, eventArgs) =>
            //        {
            //            Console.WriteLine(eventArgs.HostKeyName);
            //        };
            //        client.Connect();
            //        var command = client.RunCommand(commandStr.ToString());
            //        command.Execute();

            //        if (!string.IsNullOrWhiteSpace(command.Error))
            //        {
            //            result.Append(command.Error + "\r\n");
            //        }
            //        else
            //        {
            //            result.Append(command.Result + "\r\n");
            //        }
            //        client.Disconnect();
            //    }
            //    catch (Exception e1)
            //    {
            //        result.Append(e1.Message + "\r\n");
            //    }
            //}
            //Console.WriteLine(result);

        }



        #region preUserClass

        //var pushUrl = BaiduLiveApi.CreatePushUrl("push.lndjzz.com", "edu", Guid.NewGuid().ToString("N"),DateTime.Now.AddDays(2));
        //var stamp = BaiduLiveApi.Unix10TimeStamp(DateTime.Parse("2018/1/ 22 10:44:38"));

        //var txPushUrl = TxLiveApi.CreatePushUrl("livepush.myqcloud.com", "20460",
        //    Guid.NewGuid().ToString("N"), new DateTime(2018,1,23,23,59,59));
        //Console.WriteLine(txPushUrl);
        public enum PlayType
        {
            flv,
            rtmp,
            hls,
        }
        public class TxLiveApi
        {
            public static string CreatePlayUrl(string domain, string app, string stream, DateTime expireTime, PlayType type)
            {
                string secret = "qn4ff9kxqxh9u8o9rusne7y1qz04ufrs";
                var timeStamp = CustomUtil.Unix10TimeStamp(expireTime);
                var md5 = CustomUtil.Md5Encrypt($@"{secret}/{domain}/{app}/{stream}{expireTime}", new UTF8Encoding());
                switch (type)
                {
                    case PlayType.flv:
                        {
                            return $@"http://{domain}/{app}/{stream}.flv?timestamp={timeStamp}&secret={md5}";
                        }
                    default:
                        {
                            return $@"http://{domain}/{app}/{stream}?timestamp={timeStamp}&secret={md5}";
                        }
                }
            }
            public static string CreatePushUrl(string domain, string app, string stream, DateTime expireTime)
            {
                long unixExpire = CustomUtil.Unix10TimeStamp(expireTime);
                string secret = "56a2e2438f375ed3e8bf8ea02e7f8d51";
                var timeHex = unixExpire.ToString("x8").ToUpper();
                var token = CustomUtil.Md5Encrypt($@"{secret}{app}_{stream}{timeHex}", new UTF8Encoding());
                return $@"rtmp://{app}.{domain}/live/{app}_{stream}?bizid={app}&txSecret={token}&txTime={timeHex}";
            }
        }
        public class BaiduLiveApi
        {
            public static string CreatePlayUrl(string domain, string app, string stream, DateTime expireTime, PlayType type)
            {
                string secret = "qn4ff9kxqxh9u8o9rusne7y1qz04ufrs";
                var timeStamp = CustomUtil.Unix10TimeStamp(expireTime);
                var md5 = CustomUtil.Md5Encrypt($@"{secret}/{domain}/{app}/{stream}{expireTime}", new UTF8Encoding());
                switch (type)
                {
                    case PlayType.flv:
                        {
                            return $@"http://{domain}/{app}/{stream}.flv?timestamp={timeStamp}&secret={md5}";
                        }
                    default:
                        {
                            return $@"http://{domain}/{app}/{stream}?timestamp={timeStamp}&secret={md5}";
                        }
                }
            }
            public static string CreatePushUrl(string domain, string app, string stream, DateTime expireTime)
            {
                string expireTimeString = expireTime.ToString("s") + "Z";
                string secret = "qn4ff9kxqxh9u8o9rusne7y1qz04ufrs";
                var bytes = CustomUtil.Hmacsha256String(secret, $@"rtmp://{domain}/{app}/{stream};{expireTimeString}");
                var token = CustomUtil.Hexbytes2String(bytes);
                return $@"rtmp://{domain}/{app}/{stream}?token={token}&expire={expireTimeString}";
            }
        }
        public interface ILiveInterface
        {
            string CreatePlayUrl(string domain, string app, string stream, DateTime expireTime, PlayType type);
            string CreatePushUrl(string domain, string app, string stream, DateTime expireTime);
        }

        #endregion
    }
}
