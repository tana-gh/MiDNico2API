using System;
using System.Runtime.Serialization;
using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace MiDNicoAPI.Entity
{
#pragma warning disable 0649
    [DataContract(Name = "getplayerstatus", Namespace = "")]
    public class PlayerStatus
    {
        [DataMember(Name = "stream", Order = 0)]
        public Stream Stream { get; private set; }

        [DataMember(Name = "user", Order = 1)]
        public User User { get; private set; }

        [DataMember(Name = "rtmp", Order = 2)]
        public Rtmp Rtmp { get; private set; }

        [DataMember(Name = "ms", Order = 3)]
        public Ms Ms { get; private set; }

        [DataMember(Name = "ms_list", Order = 4)]
        public Ms[] MsList { get; private set; }

        [DataMember(Name = "marquee", Order = 5)]
        public MarqueeInfo Marquee { get; private set; }
    }

    [DataContract(Name = "stream", Namespace = "")]
    public class Stream 
    {
        [DataMember(Name = "id", Order = 0)]
        public string ID { get; private set; }

        [DataMember(Name = "title", Order = 1)]
        public string Title { get; private set; }

        [DataMember(Name = "description", Order = 2)]
        public string Description { get; private set; }

        [DataMember(Name = "provider_type", Order = 3)]
        public string ProviderType { get; private set; }

        [DataMember(Name = "default_community", Order = 4)]
        public string DefaultCommunity { get; private set; }

        [DataMember(Name = "international", Order = 5)]
        public string International { get; private set; }

        [DataMember(Name = "is_owner", Order = 6)]
        private readonly int?  _isOwner;
        public           bool IsOwner => _isOwner == null ? false : (_isOwner == 1);

        [DataMember(Name = "owner_id", Order = 7)]
        public string OwnerId { get; private set; }

        [DataMember(Name = "owner_name", Order = 8)]
        public string OwnerName { get; private set; }

        [DataMember(Name = "is_reserved", Order = 9)]
        private readonly int? _isReserved;
        public           bool IsReserved => _isReserved == null ? false : (_isReserved == 1);

        [DataMember(Name = "is_niconico_enquete_enabled", Order = 10)]
        private readonly int? _isNiconicoEnqueteEnabled;
        public           bool IsNiconicoEnqueteEnabled => _isNiconicoEnqueteEnabled == null ? false : (_isNiconicoEnqueteEnabled == 1);

        [DataMember(Name = "watch_count", Order = 11)]
        public int WatchCount { get; private set; }

        [DataMember(Name = "comment_count", Order = 12)]
        public int CommentCount { get; private set; }

        [DataMember(Name = "base_time", Order = 13)]
        public long     BaseTimeUTC { get; private set; }
        public DateTime BaseTime => DateTimeOffset.FromUnixTimeSeconds(BaseTimeUTC).LocalDateTime;

        [DataMember(Name = "open_time", Order = 14)]
        public long     OpenTimeUTC { get; private set; }
        public DateTime OpenTime => DateTimeOffset.FromUnixTimeSeconds(OpenTimeUTC).LocalDateTime;

        [DataMember(Name = "start_time", Order = 15)]
        public long     StartTimeUTC { get; private set; }
        public DateTime StartTime => DateTimeOffset.FromUnixTimeSeconds(StartTimeUTC).LocalDateTime;

        [DataMember(Name = "end_time", Order = 16)]
        public long     EndTimeUTC { get; private set; }
        public DateTime EndTime => DateTimeOffset.FromUnixTimeSeconds(EndTimeUTC).LocalDateTime;

        [DataMember(Name = "is_rerun_stream", Order = 17)]
        private readonly int? _isRerunStream;
        public bool IsRerunStream => _isRerunStream == null ? false : (_isRerunStream == 1);

        [DataMember(Name = "bourbon_url", Order = 18)]
        public string BourbonUrl { get; private set; }

        [DataMember(Name = "full_video", Order = 19)]
        public string FullVideo { get; private set; }

        [DataMember(Name = "after_video", Order = 20)]
        public string AfterVideo { get; private set; }

        [DataMember(Name = "before_video", Order = 21)]
        public string BeforeVideo { get; private set; }

        [DataMember(Name = "kickout_video", Order = 22)]
        public string KickoutVideo { get; private set; }

        [DataMember(Name = "twitter_tag", Order = 23)]
        public string TwitterTag { get; private set; }

        [DataMember(Name = "danjo_comment_mode", Order = 24)]
        public int? DanjoCommentMode { get; private set; }

        [DataMember(Name = "infinity_mode", Order = 25)]
        public int? InfinityMode { get; private set; }

        [DataMember(Name = "archive", Order = 26)]
        public int? Archive { get; private set; }

        [DataMember(Name = "press", Order = 27)]
        public PressInfo Press { get; private set; }

        [DataMember(Name = "plugin_delay", Order = 28)]
        public string PluginDelay { get; private set; }

        [DataMember(Name = "plugin_url", Order = 29)]
        public string PluginUrl { get; private set; }

        [DataMember(Name = "plugin_urls", Order = 30)]
        public string[] PluginUrls { get; private set; }

        [DataMember(Name = "allow_netduetto", Order = 31)]
        private readonly int? _allowNetDuetto;
        public bool AllowNetDuetto => _allowNetDuetto == null ? false : (_allowNetDuetto == 1);

        [DataMember(Name = "broadcast_token", Order = 32)]
        public string BroadcastToken { get; private set; }

        [DataMember(Name = "ng_scoring", Order = 33)]
        public int? NgScoring { get; private set; }

        [DataMember(Name = "is_nonarchive_timeshift_enabled", Order = 34)]
        private readonly int? _isNonarchiveTimeshiftEnabled;
        public bool IsNonarchiveTimeshiftEnabled => _isNonarchiveTimeshiftEnabled == null ? false : (_isNonarchiveTimeshiftEnabled == 1);

        [DataMember(Name = "is_timeshift_reserved", Order = 35)]
        private readonly int? _isTimeshiftReserved;
        public bool IsTimeshiftReserved => _isTimeshiftReserved == null ? false : (_isTimeshiftReserved == 1);

        [DataMember(Name = "header_comment", Order = 36)]
        public int HeaderComment { get; private set; }

        [DataMember(Name = "footer_comment", Order = 37)]
        public int FooterComment { get; private set; }

        [DataMember(Name = "split_bottom", Order = 38)]
        public int SplitBottom { get; private set; }

        [DataMember(Name = "split_top", Order = 39)]
        public int SplitTop { get; private set; }

        [DataMember(Name = "background_comment", Order = 40)]
        public int BackgroundComment { get; private set; }

        [DataMember(Name = "font_scale", Order = 41)]
        public string FontScale { get; private set; }

        [DataMember(Name = "perm", Order = 42)]
        public string Perm { get; private set; }

        [DataMember(Name = "comment_lock", Order = 43)]
        public int CommentLock { get; private set; }

        [DataMember(Name = "telop", Order = 44)]
        public TelopInfo Telop { get; private set; }

        [DataMember(Name = "picture_url", Order = 45)]
        public string PictureUrl { get; private set; }

        [DataMember(Name = "thumb_url", Order = 46)]
        public string ThumbUrl { get; private set; }



        [DataContract(Name = "press", Namespace = "")]
        public class PressInfo
        {
            [DataMember(Name = "display_lines", Order = 0)]
            public int? DisplayLines { get; private set; }

            [DataMember(Name = "display_time", Order = 1)]
            public int? DisplayTime { get; private set; }

            [DataMember(Name = "style_conf", Order = 2)]
            public string StyleConf { get; private set; }
        }

        [DataContract(Name = "telop", Namespace = "")]
        public class TelopInfo
        {
            [DataMember(Name = "enable")]
            public int _enabled;
            public bool Enabled => _enabled == 1;
        }
    }

    [DataContract(Name = "user", Namespace = "")]
    public class User 
    {
        [DataMember(Name = "user_id", Order = 0)]
        public string ID { get; private set; }

        [DataMember(Name = "nickname", Order = 1)]
        public string NickName { get; private set; }

        [DataMember(Name = "is_premium", Order = 2)]
        private readonly int _isPremium;
        public bool IsPremium => _isPremium == 1;

        [DataMember(Name = "userAge", Order = 3)]
        public int Age { get; private set; }

        [DataMember(Name = "userSex", Order = 4)]
        public int Sex { get; private set; }

        [DataMember(Name = "userDomain", Order = 5)]
        public string Domain { get; private set; }

        [DataMember(Name = "userPrefecture", Order = 6)]
        public int Prefecture { get; private set; }

        [DataMember(Name = "userLanguage", Order = 7)]
        public string Language { get; private set; }

        [DataMember(Name = "room_label", Order = 8)]
        public string RoomLabel { get; private set; }

        [DataMember(Name = "room_seetno", Order = 9)]
        public int RoomSeetNo { get; private set; }

        [DataMember(Name = "is_join", Order = 10)]
        private readonly int _isJoin;
        public bool IsJoin => _isJoin == 1;

        [DataMember(Name = "twitter_info", Order = 11)]
        public TwitterInfo Twitter { get; private set; }



        [DataContract(Name = "twitter_info", Namespace = "")]
        public class TwitterInfo
        {
            [DataMember(Name = "status")]
            public string Status { get; private set; }

            [DataMember(Name = "screen_name")]
            public string ScreenName { get; private set; }

            [DataMember(Name = "followers_count")]
            public string FollowersCount { get; private set; }

            [DataMember(Name = "is_vip")]
            private readonly int _isVIP;
            public bool IsVIP => _isVIP == 1;

            [DataMember(Name = "profile_image_url")]
            public string ProfileImageUrl { get; private set; }

            [DataMember(Name = "after_auth")]
            private readonly int _afterAuth;
            public bool AfterAuth => _afterAuth == 1;

            [DataMember(Name = "tweet_token")]
            public string TweetToken { get; private set; }
        }
    }

    [DataContract(Name = "rtmp", Namespace = "")]
    public class Rtmp 
    {
        [DataMember(Name = "url", Order = 0)]
        public string URL { get; private set; }

        [DataMember(Name = "ticket", Order = 1)]
        public string Ticket { get; private set; }
    }

    [DataContract(Name = "ms", Namespace = "")]
    public class Ms 
    {
        [DataMember(Name = "addr", Order = 0)]
        public string Address { get; private set; }

        [DataMember(Name = "port", Order = 1)]
        public int Port { get; private set; }

        [DataMember(Name = "thread", Order = 2)]
        public int Thread { get; private set; }
    }

    [DataContract(Name = "marquee", Namespace = "")]
    public class MarqueeInfo 
    {
        [DataMember(Name = "category", Order = 0)]
        public string Category { get; private set; }

        [DataMember(Name = "game_key", Order = 1)]
        public string GameKey { get; private set; }

        [DataMember(Name = "game_time", Order = 2)]
        public string GameTime { get; private set; }

        [DataMember(Name = "force_nicowari_off", Order = 3)]
        public string ForceNicowariOff { get; private set; }
    }

#pragma warning restore 0649

    public static class PlayerStatusExtension
    {
        public static IPEndPoint ToIPEndPoint(this Ms ms)
        {
            if (ms == default)         throw new ArgumentException("PlayerStatusが正しく指定されていません。");
            if (ms.Address == default) throw new ArgumentException("コメントサーバアドレスが正しく指定されていません。");

            var host    = Dns.GetHostEntry(ms.Address);
            var address = host?.AddressList?.FirstOrDefault();
            var port    = ms.Port;

            return new IPEndPoint(address, port);
        }
    }
}