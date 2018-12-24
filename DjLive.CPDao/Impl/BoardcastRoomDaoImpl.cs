using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using DjLive.CPDao.Context;
using DjLive.CPDao.Entity;
using DjLive.CPDao.Interface;
using DjLive.CPDao.Util;
using DjUtil.Tools;

namespace DjLive.CPDao.Impl
{
    public class BoardcastRoomDaoImpl : IBoardcastRoomDaoInterface
    {
        public async Task<DaoResultMessage> CreateRoom(BoardCastRoomEntity boardCastRoomEntity)
        {
            if (boardCastRoomEntity == null) return DaoResultMessage.ItemNotExist;
            DaoResultMessage message = new DaoResultMessage();
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    if (context.Domain.AsNoTracking().Any(obj => obj.SourceDomain == boardCastRoomEntity.Domain))
                    {
                        if (context.BoardCastRoom.AsNoTracking().Any(obj =>
                            obj.Id == boardCastRoomEntity.Id ||
                            (obj.StreamName == boardCastRoomEntity.StreamName 
                            //&&obj.AppName == boardCastRoomEntity.AppName && obj.Domain == boardCastRoomEntity.Domain
                            )))
                        {
                            //todo:更新
                            //context.BoardCastRoom.AddOrUpdate(boardCastRoomEntity);
                            var dbentity = context.BoardCastRoom.FirstOrDefault(obj =>
                                obj.Id == boardCastRoomEntity.Id || (obj.StreamName == boardCastRoomEntity.StreamName));
                            context.BoardCastRoom.Remove(dbentity);
                        }

                        context.BoardCastRoom.Add(boardCastRoomEntity);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        return DaoResultMessage.ItemNotExist;
                    }

                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                message.Code = DaoResultCode.UnExpectError;
                message.Message = e.Message;
            }

            return message;
        }

        public async Task<BoardCastRoomEntity> GetBoardCastRoomBySteamName(string streamName)
        {
            try
            {
                using (var context = new DjLiveCpContext())
                {
                    return await context.BoardCastRoom.AsNoTracking().FirstOrDefaultAsync(obj=>obj.StreamName.Equals(streamName));
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message, e);
                return null;
            }
        }
    }
}