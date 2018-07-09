using MiDNicoAPI.Core.Network;
using System;
using System.IO;
using System.Net;

namespace MiDNicoAPI.Core
{
    /// <summary>
    /// ニコニコの番組情報に関するAPIと通信するクラス
    /// </summary>
    public class Nico2LiveInfo
    {
        private readonly CookieContainer _cookie;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cookie">ニコニコとのCookie情報</param>
        public Nico2LiveInfo (
            CookieContainer cookie
        )
        {
            if (cookie == default) throw new ArgumentNullException(nameof(cookie));
            _cookie = cookie;
        }

        /// <summary>
        /// ニコニコの番組情報に関するAPIと通信するインスタンスを生成するメソッド.
        /// </summary>
        /// <param name="cookie">ニコニコとのCookie情報</param>
        /// <returns>Nico2LiveInfoインスタンス</returns>
        public static Nico2LiveInfo Access (
            in CookieContainer cookie
        )
        {
            if (cookie == default) throw new ArgumentNullException(nameof(cookie));
            return new Nico2LiveInfo(cookie);
        }

        /// <summary>
        /// ニコニコ生放送の番組情報を取得するメソッド.
        /// </summary>
        /// <param name="nico2LiveId">番組情報を取得する生放送ID</param>
        /// <returns>番組情報</returns>
        public Stream GetInfo (
            in string nico2LiveId
        )
        {
            var url    = $"http://live.nicovideo.jp/api/getplayerstatus?v={nico2LiveId}";
            var result = Nico2Signal.Get(url, _cookie);
            return result.Content.ReadAsStreamAsync().Result;
        }

        /// <summary>
        /// ニコニコ生放送の番組にコメント投稿するための投稿キーを取得するメソッド.
        /// </summary>
        /// <param name="nico2Thread">ニコニコ生放送のスレッド番号</param>
        /// <param name="commentBlockNumber">(最新コメント番号 / 100) した値</param>
        /// <returns>コメント投稿用のキー値</returns>
        public string GetPostKey (
            in int nico2Thread, 
            in int commentBlockNumber
        )
        {
            if (nico2Thread        < 0) throw new ArgumentOutOfRangeException(nameof(nico2Thread));
            if (commentBlockNumber < 0) throw new ArgumentOutOfRangeException(nameof(commentBlockNumber));

            var url = $"http://live.nicovideo.jp/api/getpostkey?thread={nico2Thread}&block_no={commentBlockNumber}";
            var result = Nico2Signal.Get(url, _cookie);
            return result?.Content?.ReadAsStringAsync().Result.Replace("postkey=", "");
        }

        /// <summary>
        /// ニコニコ生放送の概要情報を取得するメソッド.
        /// </summary>
        /// <param name="ipendpoint">ニコニコのIPEndPoint</param>
        /// <param name="nico2Thread">ニコニコ生放送のスレッド番号</param>
        /// <returns>生放送概要情報</returns>
        public byte[] GetThreadInfo (
            IPEndPoint ipendpoint,
            int        nico2Thread
        )
        {
            if (ipendpoint  == default) throw new ArgumentNullException(nameof(ipendpoint));
            if (nico2Thread == default) throw new ArgumentOutOfRangeException(nameof(nico2Thread));

            var client = Nico2Comment.Create(_cookie, ipendpoint);

            byte[] threadInfo = default;
            var _ = client.ConnectAsync(nico2Thread, 
                res =>
                {
                    threadInfo = res;
                    client.DisConnect();
                }
                , 512, 0).Result;

            return threadInfo;
        }
    }
}
