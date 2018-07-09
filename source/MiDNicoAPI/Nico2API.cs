using System;
using System.Collections.Generic;
using IO = System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MiDNicoAPI.Core;
using MiDNicoAPI.Data.Mapper;
using MiDNicoAPI.Entity;

namespace MiDNicoAPI
{
    public static class Nico2API
    {
        private static readonly Dictionary<IPEndPoint, Nico2Comment> CACHE = new Dictionary<IPEndPoint, Nico2Comment>();
        private static readonly string DWANGO_STAFF_ID = "900000000";

        /// <summary>
        /// (API: login)
        /// ニコニコにログインする.
        /// </summary>
        /// <param name="email">ニコニコログイン用メールアドレス</param>
        /// <param name="password">ニコニコログイン用パスワード</param>
        /// <returns>ニコニコのログインCookie情報</returns>
        public static CookieContainer Login (
            in string email, 
            in string password
        )
        {
            return Nico2Auth.Login(email, password);
        }

        /// <summary>
        /// (API: getplayerstatus)
        /// 指定した生放送番組情報、及びユーザ情報を取得するAPI.
        /// </summary>
        /// <param name="cookie">ニコニコのCookie情報を指定.</param>
        /// <param name="nico2LiveId">'lv'または'co'から始まるニコニコIDを指定する.</param>
        /// <returns>番組・ユーザ情報</returns>
        public static PlayerStatus GetPlayerStatus (
            in CookieContainer cookie,
            in string          nico2LiveId
        )
        {
            // ニコニコAPIを利用して, 番組情報を取得する.
            IO.Stream data   = Nico2LiveInfo.Access(cookie).GetInfo(nico2LiveId);

            // byte[]のままだと使いにくいため, PlayerStatusクラスにマッピングしてからreturn.
            IMapper<PlayerStatus> mapper = new Nico2XmlDataContractMapper<PlayerStatus>();
            return mapper.Map(data);
        }

        /// <summary>
        /// (API: getpostkey)
        /// 指定した生放送番組にコメントを投稿するためのキーを取得する.
        /// </summary>
        /// <param name="cookie">ニコニコのCookie情報を指定.</param>
        /// <param name="thread">番組</param>
        /// <returns></returns>
        public static (string key, ThreadInfo thread) GetPostKey(
            in CookieContainer cookie,
            in ThreadInfo thread
        )
        {
            var key = Nico2LiveInfo.Access(cookie).GetPostKey(thread.Thread, thread.BlockNo);
            return (key, thread);
        }

        /// <summary>
        /// (非公式API: consumeComment)
        /// 生放送番組とのセッション情報からコメントを取得するAPI.
        /// </summary>
        /// <param name="ipEndPoint"></param>
        /// <param name="cookie">ニコニコのCookie情報を指定.</param>
        /// <param name="nico2Thread">Thread番号</param>
        /// <param name="bufSize"></param>
        /// <param name="historyFrom"></param>
        /// <param name="observer"></param>
        /// <returns></returns>
        public static async Task<Nico2Comment> ConsumeComment (
            (IPEndPoint ipEndPoint, CookieContainer cookie, int nico2Thread) session,
            Action<LiveComment> observer    = null,
            short               historyFrom = 100 ,
            int                 bufSize     = 1024
            
        )
        {
            if (!CACHE.TryGetValue(session.ipEndPoint, out Nico2Comment client)) 
            {
                client = Nico2Comment.Create(session.cookie, session.ipEndPoint);
                CACHE.Add(session.ipEndPoint, client);
            }

            /// <summary>
            ///  コメントを受信した際のルーチン
            /// </summary>
            /// <param name="data">受信したデータ</param>
            /// <param name="prev">前回の受信データのキャッシュ</param>
            void RecievedCommentRoutine(
                byte[] data,
                byte[] prev
            )  
            {
                string prevStr = prev?.ToUTF8String() ?? "";
                string xml     = prevStr + data.ToUTF8String();
                string[] lines = xml.ToLines();
                prev = null;

                IMapper<MiDComment> mapper = new Nico2XmlSerializeMapper<MiDComment>();
                foreach (var line in lines)
                {
                    if (!(line.EndsWith(">") && line.Contains("</")))
                    {
                        prev = line.ToBinary();
                    }

                    var mapped = mapper.Map(line);
                    if (mapped == default)
                    {
                        continue;
                    }

                    var comment = new LiveComment(mapped);
                    observer?.Invoke(comment);

                    // 運営からdisconnectコマンドを受け取った場合の処理
                    if (mapped.UserId == DWANGO_STAFF_ID && mapped.Text.Contains("/disconnect"))
                    {
                        client.DisConnect();
                    }
                }
            }

            byte[] prevData = null;
            await client.ConnectAsync(session.nico2Thread,
                                      data => RecievedCommentRoutine(data, prevData),
                                      bufSize, 
                                      historyFrom
                                     );
            return client;
        }

        /// <summary>
        /// (非公式API: stopComment)
        /// 生放送番組とのセッション情報からコメントを取得するを停止するAPI.
        /// </summary>
        /// <param name="ipEndPoint"></param>
        public static void StopComment (
            in IPEndPoint ipEndPoint
        )
        {
            if (CACHE.TryGetValue(ipEndPoint, out Nico2Comment client))
            {
                client.DisConnect();
            }
        }

        /// <summary>
        /// (非公式API: getThreadInfo)
        /// 生放送番組とのセッション情報からスレッド概要を取得するAPI.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static ThreadInfo GetThreadInfo (
            in (IPEndPoint ipEndPoint, CookieContainer cookie, int nico2Thread) session
        )
        {
            var threadInfo = Nico2LiveInfo.Access(session.cookie)
                                          .GetThreadInfo(session.ipEndPoint, session.nico2Thread);
            IMapper<MiDThreadInfo> mapper = new Nico2XmlSerializeMapper<MiDThreadInfo>();

            var encoded = Encoding.UTF8.GetString(threadInfo);
            var mapped  = mapper.Map(encoded);
            return new ThreadInfo(mapped);
        }

        /// <summary>
        /// (非公式API: postComment)
        /// 生放送番組に対してコメントを投稿するAPI.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="postkey"></param>
        /// <param name="userInfo"></param>
        /// <param name="message"></param>
        /// <param name="startdate">生放送開始時刻</param>
        /// <param name="commands"></param>
        public static void PostComment (
            in (IPEndPoint  ipEndPoint, CookieContainer cookie, int nico2Thread) session,
            in (string      key       , ThreadInfo      thread   ) postkey ,
            in (string      userId    , bool            isPremium) userInfo,
            in  string      message   ,
            in  DateTime    startdate ,
            params string[] commands
        )
        {
            Nico2Comment.Create(session.cookie, session.ipEndPoint)
                        .Send  (message,
                                postkey.key,
                                userInfo.userId, 
                                postkey.thread.Ticket, 
                                session.nico2Thread,
                                startdate,
                                userInfo.isPremium,
                                commands);
        }

        /// <summary>
        /// (非公式API: postCommentAsAdmin)
        /// 生放送番組に対して放送主権限でコメントを投稿するAPI.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="nico2liveId"></param>
        /// <param name="message"></param>
        /// <param name="isPermanent"></param>
        /// <param name="color"></param>
        /// <param name="nickname"></param>
        public static void PostCommentAsAdmin (
            in (IPEndPoint ipEndPoint, CookieContainer cookie, int nico2Thread) session,
            in int    nico2liveId,
            in string message,
            in bool   isPermanent = false,
            in string color       = "white",
            in string nickname    = ""
        )
        {
            Nico2Comment.Create(session.cookie, session.ipEndPoint)
                        .SendAsAdmin(message, nico2liveId, isPermanent, nickname, color);
        }

        /// <summary>
        /// (非公式API: deletePermComment)
        /// permコメントを削除するAPI.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="nico2liveId"></param>
        public static void DeletePermComment (
            in (IPEndPoint ipEndPoint, CookieContainer cookie, int nico2Thread) session,
            in int nico2liveId
        )
        {
            Nico2Comment.Create(session.cookie, session.ipEndPoint)
                        .DeletePermComment(nico2liveId);
        }

        /// <summary>
        /// (API: getuserinfo)
        /// 指定したユーザＩＤからユーザ情報を取得する.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static UserInfo GetUserInfo (
            in string userId
        )
        {
            // ユーザIDをintに変換できない場合, 184ユーザとして処理する.
            if(!int.TryParse(userId, out int id))
            {
                return new UserInfo(userId, "184", "", -1, null);
            }

            // ユーザIDが運営IDと一致した場合, 運営として処理する.
            if (userId == DWANGO_STAFF_ID)
            {
                return new UserInfo(userId, "運営", "", -1, null);
            }

            // 通常コメントの場合のみ, ユーザ情報を取得する.
            // それ以外の場合に実行すると NullExceptionが発生する.
            var info = Nico2UserInfo.Take(userId);
            IMapper<UserInfo> mapper = new Nico2XmlDataContractMapper<UserInfo>();
            return mapper.Map(info);
        }
    }

    internal static class Extensions
    {
        internal static byte[] ToBinary (
            this string str
        )
        {
            return Encoding.UTF8.GetBytes(str);
        }

        internal static string ToUTF8String (
            this byte[] bin
        )
        {
            return Encoding.UTF8.GetString(bin);
        }

        internal static string[] ToLines (
            this string str
        )
        {
            return str.Split('\0')?.Where(l => 0 < l?.Length)?.ToArray();
        }
    }
}
