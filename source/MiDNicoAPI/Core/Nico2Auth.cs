using MiDNicoAPI.Core.Network;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace MiDNicoAPI.Core
{
    /// <summary>
    /// ニコニコとの認証に関するAPIと通信するクラス
    /// </summary>
    public static class Nico2Auth
    {
        /// <summary>
        /// ニコニコにログインするメソッド.
        /// </summary>
        /// <param name="email">ニコニコログイン用メールアドレス</param>
        /// <param name="password">ニコニコログイン用パスワード</param>
        /// <returns></returns>
        public static CookieContainer Login (
            in string email, 
            in string password
        )
        {
            if (string.IsNullOrWhiteSpace(email)   ) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            var param = new Dictionary<string, string>
                             {
                                 { "next_url", ""        },
                                 { "mail"    , email     },
                                 { "password", password  },
                             };
            var content = new FormUrlEncodedContent(param);
            string api  = @"https://secure.nicovideo.jp/secure/login?site=nicolive";

            return Nico2Signal.TakeCookie(api, content);
        }
    }
}
