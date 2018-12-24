using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using DjLive.CPDao.Context;
using DjLive.CPDao.Migrations;
using DjUtil.Tools;
using Unity;

namespace DjLive.ControlPanel
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            DataBaseInitialize.Initialize();
            //注册全局异常处理 拦截器
            GlobalConfiguration.Configuration.Filters.Add(new ApiExceptionFilterAttribute());
            //注册全局身份验证 拦截器
            GlobalConfiguration.Configuration.Filters.Add(new ApiAuthFilterAttribute());
            //注册全局Action处理 拦截器
            GlobalConfiguration.Configuration.Filters.Add(new ApiActionExcuteFilterAttribute());

            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

           // 注册Unity注入组件
           // var container = new UnityContainer();
           // DependencyRegisterType.Container_Sys(ref container);
           // DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            {
                Response.StatusCode = 200;
                Response.Flush();
            }
        }
    }
    
}