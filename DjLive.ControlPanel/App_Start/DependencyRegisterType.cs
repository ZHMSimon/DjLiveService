using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using Unity;

namespace DjLive.ControlPanel
{
    public class DependencyRegisterType
    {
        //系统注入
        public static void Container_Sys(ref UnityContainer container)
        {
            UnityConfigurationSection config = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);
            config.Configure(container, "MyContainer");
        }
    }
}