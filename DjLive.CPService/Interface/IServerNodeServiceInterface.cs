using System.Collections.Generic;
using System.Threading.Tasks;
using DjLive.CPService.Util;
using DjLive.SdkModel.Model;

namespace DjLive.CPService.Interface
{
    public interface IServerNodeServiceInterface
    {
        Task<int> GetServerNodesCount(string userId, ServerNodeModel delta);
        Task<List<ServerNodeModel>> GetSimpleServerNodes(string userId, ServerNodeModel delta);
        Task<ServerNodeModel> FindByIdAsync(string userId, string id);
        Task<ServiceResultMessage> UpdateServerNode(string userId, string id, ServerNodeModel model);
        Task<bool> ServerNodeModelExists(string userId, string id);
        Task<ServiceResultMessage> AddServerNode(string userId, string id, ServerNodeModel model);
        Task<ServiceResultMessage> RemoveAsync(string userId, string id);

        Task<ServiceResultMessage> UpdateSrsConf(string userId, string id ="");
    }
}