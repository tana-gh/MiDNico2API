using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MiDNicoAPI.Entity
{
    public class LiveComment
    {
        internal LiveComment (
            MiDComment comment
        )
        {
            this.Thread      = comment.Thread;
            this.No          = comment.CommentNumber;
            this.VposUTC     = comment.Vpos;
            this.DateUTC     = comment.DateUTC;
            this.DateUsec    = comment.DateUsec;
            this.CommandCode = comment.MailCode;
            this.UserId      = comment.UserId;
            this.Anonymity   = comment.Anonymity == 1;
            this.IsPremium   = comment.Premium   == 1;
            this.Text        = comment.Text;
        }

        public int       Thread      { get; }
        public int       No          { get; }
        public long      VposUTC     { get; }
        public TimeSpan  Vpos => TimeSpan.FromSeconds(VposUTC / 100);
        public long      DateUTC     { get; }
        public DateTime  Date => DateTimeOffset.FromUnixTimeSeconds(DateUTC).LocalDateTime;
        public long      DateUsec    { get; }
        public string    CommandCode { get; }
        public string    UserId      { get; }
        public bool      Anonymity   { get; }
        public bool      IsPremium   { get; }
        public string    Text        { get; }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [XmlRoot("chat")]
    public class MiDComment
    {
        [XmlAttribute("thread")]    public int    Thread        { get; set; }
        [XmlAttribute("no")]        public int    CommentNumber { get; set; }
        [XmlAttribute("vpos")]      public long   Vpos          { get; set; }
        [XmlAttribute("date")]      public long   DateUTC       { get; set; }
        [XmlAttribute("date_usec")] public long   DateUsec      { get; set; }
        [XmlAttribute("mail")]      public string MailCode      { get; set; }
        [XmlAttribute("user_id")]   public string UserId        { get; set; }
        [XmlAttribute("anonymity")] public short  Anonymity     { get; set; }
        [XmlAttribute("premium")]   public short  Premium       { get; set; }
        [XmlText]                   public string Text          { get; set; }
    }
}
