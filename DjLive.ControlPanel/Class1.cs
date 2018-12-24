using System.Collections;
using System.Threading;
using DjUtil.Tools;

namespace DjLive.ControlPanel
{
    //todo:设计为简单计时器 threadPool 任务分发 队列执行
    public interface ISchedulerTaskHandler
    {
        
    }
    public class SchedulerTask
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }
    public class Scheduler: Singleton<Scheduler>
    {
        static Queue defaultTaskQueue = new Queue();
        Queue threadSafaTaskQueue = Queue.Synchronized(defaultTaskQueue);
        protected override void Init()
        {
            base.Init();
        }

        public void ProcessPostTask(string url,string json)
        {
            Thread processer = new Thread(() =>
            {
                var task =HttpUtil.PostAsync(url, json).ContinueWith(tsk =>
                {
                    
                });
            });
            processer.Start();
        }
    }
}