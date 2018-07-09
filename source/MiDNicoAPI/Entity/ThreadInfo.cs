using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MiDNicoAPI.Entity
{
    public class ThreadInfo
    {
        internal ThreadInfo (
            MiDThreadInfo info
        )
        {
            this.ResultCode    = info.ResultCode;
            this.Thread        = info.Thread;
            this.LastResponse  = info.LastResponse;
            this.Ticket        = info.Ticket;
            this.Revision      = info.Revision;
            this.ServerTimeUTC = info.ServerTime;
        }

        public int      ResultCode    { get; }
        public int      Thread        { get; }
        public int      LastResponse  { get; }
        public int      BlockNo => LastResponse / 100;
        public string   Ticket        { get; }
        public int      Revision      { get; }
        public long     ServerTimeUTC { get; }
        public DateTime ServerTime => DateTimeOffset.FromUnixTimeSeconds(ServerTimeUTC).LocalDateTime;
    }


    [EditorBrowsable(EditorBrowsableState.Never)]
    [XmlRoot("thread")]
    public class MiDThreadInfo
    {
        [XmlAttribute("resultcode" )] public int    ResultCode   { get; set; }
        [XmlAttribute("thread"     )] public int    Thread       { get; set; }
        [XmlAttribute("last_res"   )] public int    LastResponse { get; set; }
        [XmlAttribute("ticket"     )] public string Ticket       { get; set; }
        [XmlAttribute("revision"   )] public int    Revision     { get; set; }
        [XmlAttribute("server_time")] public long   ServerTime   { get; set; }
    }
}
