using System.Threading.Tasks;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Util;

namespace DjLive.CPDao.Interface
{
    public interface IBoardcastRoomDaoInterface
    {
        Task<DaoResultMessage> CreateRoom(BoardCastRoomEntity boardCastRoomEntity);
        Task<BoardCastRoomEntity> GetBoardCastRoomBySteamName(string streamName);
    }
}