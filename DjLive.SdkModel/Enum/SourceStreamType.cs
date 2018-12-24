using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DjLive.SdkModel.Enum
{
    public enum SourceStreamType
    {
        Rtmp_Push = 1,

        //Stream Caster
        Rtsp_Push = 1 << 1,
        Http_Flv_Push = 1 << 2,
        MpegTs_Over_Udp_Push = 1 << 3,

        //VHost->Ingest
        Rtmp_Ingset = 1 << 4,
        Flv_Ingsert = 1 << 5,
        Hls_Ingsert = 1 << 6,
    }
}
