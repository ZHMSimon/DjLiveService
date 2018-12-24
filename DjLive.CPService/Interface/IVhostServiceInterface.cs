using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPService.Util;
using DjLive.SdkModel.Model;



namespace DjLive.CPService.Interface
{
    public interface IVhostServiceInterface
    {
        Task<ServiceResultMessage> CreateVhost(string userId, DomainModel model);
        Task<int> GetDomainsCount(string userId, DomainModel delta);
        Task<List<DomainModel>> GetSimpleDomains(string userId, int page, int countPerPage, DomainModel delta);
        Task<DomainModel> FindByIdAsync(string userId, string id);
        Task<ServiceResultMessage> UpdateDomain(string userId, string id, DomainModel domainModel);
        Task<bool> DomainModelExists(string userId, string id);
        Task<ServiceResultMessage> RemoveAsync(string userId, string id);
        Task<ServiceResultMessage> CreateLiveRoom(string userId,BoardCastRoomModel model);
        Task<BoardCastRoomModel> GetLiveRoomByStreamName(string objStream);
    }
}