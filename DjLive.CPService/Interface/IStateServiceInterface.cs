using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.Control.Model.ConfModel.LiveService;
using DjLive.Control.Model.WebModel.Data;
using DjLive.CPService.Util;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;

namespace DjLive.CPService.Interface
{
    public interface IStateServiceInterface
    {
        Task<List<LiveVHostModel>> GetVhostsState();
        Task<List<LiveStreamModel>> GetStreamsState();
        Task<List<LiveClientModel>> GetClientsState();
        void SaveVodItem(CallbackResponseBase callbackResponseBase);
        bool CheckRoomExist(string streamName);
        Task<List<VodItemModel>> GetVodItems(string domainId, string streamName, int page, int countPerPage);
    }
}