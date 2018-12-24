using System;

namespace DjLive.Sdk.Util
{
    public class Singleton<TS>
    {
        protected static TS m_privateInstance;
        protected static object locker = new object();
        protected Singleton()
        {
            Init();
        }

        protected virtual void Init()
        { }

        public static TS GetInstance()
        {
            lock (locker)
            {
                if (m_privateInstance == null)
                {
                    m_privateInstance = (TS)((object)Activator.CreateInstance(typeof(TS)));
                }
                return m_privateInstance;
            }
            
        }
    }
}