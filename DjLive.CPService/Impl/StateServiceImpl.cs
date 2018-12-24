using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DjLive.Control.Service;
using DjLive.Control.Service.Impl;
using DjLive.CPDao.Impl;
using DjLive.CPDao.Interface;
using DjLive.CPService.Interface;
using DjLive.SdkModel;
using DjLive.SdkModel.Model;

namespace DjLive.CPService.Impl
{
    public class StateServiceImpl:IStateServiceInterface
    {
        private IDjLiveInterface LiveService { get; set; } = new DjLiveService();
        private IVodItemDaoInterface VodItemDao { get; set; } = new VodItemDaoImpl();
        private IBoardcastRoomDaoInterface BoardcastRoomDao { get; set; } = new BoardcastRoomDaoImpl();

        public async Task<List<LiveVHostModel>> GetVhostsState()
        {
            try
            {
                var root = await LiveService.GetVhostInfo();
                var list = root?.vhosts?.Select(item => new LiveVHostModel()
                {
                    SrsHostId = item.id,
                    SrsServerId = root.server,
                    Name = item.name,
                    Enable = item.enabled == "true",
                    ClientCount = item.clients,
                    StreamCount = item.streams,
                    SendFlowKB = item.send_bytes/1024/8,
                    RecvFlowKB = item.recv_bytes/1024/8,
                    SendFlowRateKBs = item.kbps.send_30s/8,
                    RecvFlowRateKBs = item.kbps.recv_30s/8,
                    IsHaveHls = item.hls?.enabled == "true",
                });
                return list?.ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<List<LiveStreamModel>> GetStreamsState()
        {
            try
            {
                var root = await LiveService.GetStreamInfo();
                var list = root?.streams?.Select(item => new LiveStreamModel()
                {
                    SrsHostId = item.vhost,
                    SrsStreamId = item.id,
                    SrsServerId = root.server,
                    Name = item.name,
                    App = item.app,
                    ClientCount = item.clients,
                    SendFlowKB = item.send_bytes / 1024 / 8,
                    RecvFlowKB = item.recv_bytes / 1024 / 8,
                    SendFlowRateKBs = item.kbps.send_30s / 8,
                    RecvFlowRateKBs = item.kbps.recv_30s / 8,
                    Active = item.publish?.active == "true",
                    VideoCode = item.video?.codec,
                    VideoProfile = item.video?.profile,
                    VideoLevel = item.video?.level,
                    AudioCode = item.audio?.codec,
                    AudioSampleRate = item.audio?.sample_rate,
                    AudioChannel = item.audio?.channel,
                    AudioProfile = item.audio?.profile,
                });
                return list?.ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<List<LiveClientModel>> GetClientsState()
        {
            try
            {
                var root = await LiveService.GetClientInfo();
                var list = root?.clients?.Select(item => new LiveClientModel()
                {
                    SrsClientId = item.id,
                    SrsHostId = item.vhost,
                    SrsStreamId = item.id,
                    SrsServerId = root.server,
                    ClientIp = item.ip,
                    PageUrl = item.pageUrl,
                    SwfUrl = item.swfUrl,
                    TcUrl = item.tcUrl,
                    Url = item.url,
                    Type = item.type == "Play"? LiveClientModel.LiveClientType.播放:item.type == "fmle-publish"? LiveClientModel.LiveClientType.推流: LiveClientModel.LiveClientType.未知,
                    IsPublish = item.publish == "true",
                    AliveTime = TimeSpan.FromMilliseconds(item.alive).ToString("g"),
                });
                return list?.ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void SaveVodItem(CallbackResponseBase callbackResponseBase)
        {
            throw new NotImplementedException();
        }

        public bool CheckRoomExist(string streamName)
        {
            throw new NotImplementedException();
        }

        public Task<List<VodItemModel>> GetVodItems(string domainId, string streamName, int page, int countPerPage)
        {
            throw new NotImplementedException();
        }
    }
}