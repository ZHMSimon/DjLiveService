using System;

namespace DjLive.CPService.Interface
{
    public interface IInitializtionInterface
    {
        void InstallLiveService(Action<int> progressCallback);
    }
}