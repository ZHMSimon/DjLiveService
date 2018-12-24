//using DjLive.ControlPanel;

//[assembly: OwinStartup(typeof(SignalRStartup))]

//namespace DjLive.ControlPanel
//{
//    public class SignalRStartup
//    {
//        public void Configuration(IAppBuilder app)
//        {
//            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888
//            //app.Map("/chat", map =>
//            //{
//            //    var hubConfiguration = new HubConfiguration()
//            //    {
//            //        EnableJSONP = true
//            //    };
//            //    map.RunSignalR(hubConfiguration);
//            //});
//            app.Map("/message", map =>
//            {
//                var hubConfiguration = new HubConfiguration()
//                {
//                    EnableJSONP = true
//                };
//                map.RunSignalR(hubConfiguration);
//            });
//        }
//    }
//}
