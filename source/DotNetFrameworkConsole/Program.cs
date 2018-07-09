namespace DotNetFrameworkConsole
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using MiDCookieGetter;
    using MiDNicoAPI;
    using MiDNicoAPI.Entity;

    class Program
    {
        #region 個人情報
        static string email = "___";
        static string pass = "___";
        #endregion

        static int _liveId = 314335528;
        static Dictionary<string, string> _kotehan = new Dictionary<string, string>();
        static string _github = "https://github.com/Midoliy/MiDNico2API";


        static void Main(string[] args)
        {
            // ======================================================================================================================================
            // ニコニコにログインしてCookieを取得するサンプル
            // ======================================================================================================================================
            //var cookie = Nico2API.Login(email, pass);


            // ======================================================================================================================================
            // Google Chromeからクッキー情報を取得するサンプル
            // ======================================================================================================================================
            var table = ChromeCookieGetter.Prepare    (ChromeCookieGetter.DefaultWindowsPath)   // Googole Chrome のCookie DBの場所を指定
                                          .WithHostKey(".nicovideo.jp")                         // ニコニコのCookieを取得するためのKeyを指定
                                          .WithName   ("user_session")                          // ニコニコのCookieの中でもSession情報が必要なので, それを取得するためのKeyを指定
                                          .Do();
            var cookie = new ChromeNicoCookie(table);                                           // 取得したDB情報を元にCookieデータを生成する



            // ======================================================================================================================================
            // Mozilla FireFoxからクッキー情報を取得するサンプル
            // ======================================================================================================================================
            //var table = FireFoxCookieGetter.Prepare    (FireFoxCookieGetter.DefaultWindowsPath)   // Mozilla FireFox のCookie DBの場所を指定
            //                               .WithHostKey(".nicovideo.jp")                          // ニコニコのCookieを取得するためのKeyを指定
            //                               .WithName   ("user_session")                           // ニコニコのCookieの中でもSession情報が必要なので, それを取得するためのKeyを指定
            //                               .Do();
            //var cookie = new FireFoxNicoCookie(table);                                            // 取得したDB情報を元にCookieデータを生成する



            // ======================================================================================================================================
            // ニコニコ生放送の番組情報を取得するサンプル
            // ======================================================================================================================================
            PlayerStatus playerstatus = Nico2API.GetPlayerStatus(cookie, $"lv{_liveId}");

            // ------------------------------------------
            // パラメータ作成用の情報を取得
            string     userId      = playerstatus.User.ID;
            bool       ispremium   = playerstatus.User.IsPremium;
            IPEndPoint ipendpoint  = playerstatus.Ms.ToIPEndPoint();
            int        nico2thread = playerstatus.Ms.Thread;

            // ------------------------------------------
            // 各種APIで利用するパラメータを作成
            var user    = (userId, ispremium);                 // ユーザ情報
            var session = (ipendpoint, cookie, nico2thread);   // セッション情報



            // ======================================================================================================================================
            // 番組からコメントを取得するサンプル
            // ======================================================================================================================================
            var task = Nico2API.ConsumeComment(session,         // [必須] セッション情報
                                               comment =>       // [必須] コメント受信時の処理
                                               {
                                                   var number  = comment.No;                // コメント番号
                                                   var userid  = comment.UserId;            // コメント投稿者ユーザID
                                                   var text    = comment.Text;              // コメント内容
                                                   var _184    = comment.Anonymity;         // 184かどうか
                                                   var date    = comment.Date;              // コメント投稿時刻
                                                   var vpos    = comment.Vpos;              // コメント投稿時間(生放送が開始してからの経過時間)
                                                   var command = comment.CommandCode;       // コメントに設定されているコマンド(whiteやredなどの色指定等)

                                                   // ------------------------------------------
                                                   // ユーザ情報を取得するサンプル
                                                   if (!_kotehan.TryGetValue(userid, out string name))
                                                   {
                                                       var userinfo = Nico2API.GetUserInfo(userid);  // ユーザ情報を取得するAPI
                                                       _kotehan.Add(userid, userinfo.User.NickName);
                                                       name = userinfo.User.NickName;
                                                   }

                                                   // ------------------------------------------
                                                   // コメントの出力
                                                   Console.WriteLine($"{number} {name} {text} ({vpos})");
                                               },
                                               200,             // [オプション] 過去何件分まで遡ってコメントを取得するかの設定(default:100件)
                                               5120             // [オプション] 一度に何byte分のコメント情報を取得するかの設定(default:1024byte)
                                              );



            // ======================================================================================================================================
            // 番組にコメントを投稿するサンプル
            // ======================================================================================================================================
            var thread = Nico2API.GetThreadInfo(session);            // 生放送の概要を取得する
            var postkey = Nico2API.GetPostKey(cookie, thread);     // コメントを投稿するためのキーを取得する
            var startdate = playerstatus.Stream.BaseTime;               // 生放送の開始時刻を取得する

            Nico2API.PostComment(session,        // [必須]       セッション情報
                                 postkey,        // [必須]       コメント投稿用キー値
                                 user,           // [必須]       ユーザ情報
                                 _github,        // [必須]       投稿するコメント内容
                                 startdate,      // [必須]       生放送の開始時刻
                                 "red",          // [オプション] 文字色
                                 "small",        // [オプション] 文字サイズ
                                 "naka"          // [オプション] 表示位置
                                );



            // ======================================================================================================================================
            // 番組に運営者コメントを投稿するサンプル
            // ======================================================================================================================================
            Nico2API.PostCommentAsAdmin(session,          // [必須]       セッション情報
                                        _liveId,          // [必須]       生放送ID
                                        _github,          // [必須]       投稿するコメント内容
                                        true,             // [オプション] 表示を生主コメント欄に固定するかどうか(default:false)
                                        "blue2",          // [オプション] 文字色(default:white)
                                        "管理人"          // [オプション] コメントに付与するニックネーム(default:"")
                                       );

            //Nico2API.DeletePermComment(session, _liveId); // 生主コメント欄に固定しているコメントを削除する



            // ======================================================================================================================================
            // コメント取得を停止するサンプル
            // ======================================================================================================================================
            Console.ReadKey();
            Nico2API.StopComment(session.ipendpoint);

            Console.WriteLine("\n##### END #####");
            Console.ReadKey();
        }
    }
}
