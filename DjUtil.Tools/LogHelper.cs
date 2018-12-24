using System;
using System.Diagnostics;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace DjUtil.Tools
{
    public class LogHelper
    {
        private static LogHelper logHelper = null;
        public ILog Log { get; set; }
        private LogHelper()
        {
            Log = log4net.LogManager.GetLogger(typeof(string));
        }

        public static LogHelper GetInstance()
        {
            if (logHelper == null)
            {
                logHelper = new LogHelper();
            }
            return logHelper;
        }
        public static void Error(string msg)
        {
            try
            {
                //获取调用方法
                //StackTrace trace = new StackTrace();
                //var a =  trace.GetFrame(1).GetMethod();
                log4net.ILog log = log4net.LogManager.GetLogger(typeof(String));
                log.Error( msg);
            }
            catch (Exception e)
            {
                Trace.TraceWarning(e.ToString());
            }
        }
        public static void Error(string msg, Exception ex)
        {
            try
            {
                //获取调用方法
                //StackTrace trace = new StackTrace();
                //var a =  trace.GetFrame(1).GetMethod();
                log4net.ILog log = log4net.LogManager.GetLogger(typeof(String));
                log.Error( msg, ex);
            }
            catch (Exception e)
            {
                Trace.TraceWarning(e.ToString());
            }

        }
        public static string ErrorWithId(string msg)
        {
            try
            {
                //获取调用方法
                //StackTrace trace = new StackTrace();
                //var a =  trace.GetFrame(1).GetMethod();
                var id = Guid.NewGuid().Str();
                log4net.ILog log = log4net.LogManager.GetLogger(typeof(String));
                log.Error($"ErrorId: {id} \r\n" + msg);
                return id;
            }
            catch (Exception e)
            {
                Trace.TraceWarning(e.ToString());
                return "Logger except!";
            }
        }
        public static string ErrorWithId(string msg, Exception ex)
        {
            try
            {
                //获取调用方法
                //StackTrace trace = new StackTrace();
                //var a =  trace.GetFrame(1).GetMethod();
                var id = Guid.NewGuid().Str();
                log4net.ILog log = log4net.LogManager.GetLogger(typeof(String));
                log.Error($"ErrorId: {id} \r\n"  + msg, ex);
                return id;
            }
            catch (Exception e)
            {
                Trace.TraceWarning(e.ToString());
                return "Logger except!";
            }
            
        }
        public static void Debug(string msg)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(typeof(String));
                log.Debug(msg);
            }
            catch (Exception e)
            {
                Trace.TraceWarning(e.ToString());
            }
            
        }

        public static void Fatal(string msg)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(typeof(String));
                log.Fatal(msg);
            }
            catch (Exception e)
            {
                Trace.TraceWarning(e.ToString());
            }
            
        }

        public static void Info(string msg)
        {
            try
            {
                log4net.ILog log = log4net.LogManager.GetLogger(typeof(String));
                log.Info(msg);
            }
            catch (Exception e)
            {
                Trace.TraceWarning(e.ToString());
            }

            
        }
        public static void Info(string msg, params object[] args)
        {
            try
            {
                foreach (var item in args)
                {
                    msg += item + ";";
                }
                Info(msg);
            }
            catch (Exception e)
            {
                Trace.TraceWarning(e.ToString());
            }
            
        }

        //private static string FormatMsg(string Id,string msg)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (Threadstatic)
        //    {
                
        //    }
        //}
    }

}
