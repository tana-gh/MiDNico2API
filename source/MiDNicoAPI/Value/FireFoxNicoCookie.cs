namespace MiDNicoAPI
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Net;

    /// <summary>
    /// Mozilla FireFoxのCookie DBから取得した情報から,
    /// ニコニコ通信用のCookie情報を生成し, 管理するクラス.
    /// </summary>
    public class FireFoxNicoCookie : CookieContainer
    {
        private static readonly string _errorMessage = "Mozilla FireFoxのCookie情報を指定してください。";

        public FireFoxNicoCookie (
            in DataTable chromeCookieTable
        )
        {
            if (chromeCookieTable == null)         throw new ArgumentException("Cookie情報にnullは指定できません。");
            if (chromeCookieTable.Rows.Count <= 0) throw new ArgumentException("Cookie情報に空データは指定できません。");

            var cols = chromeCookieTable.Columns.OfType<DataColumn>().ToArray();
            if (!cols.Any(c => c.ColumnName == "id"              )) throw new ArgumentException(_errorMessage + ": id");
            if (!cols.Any(c => c.ColumnName == "baseDomain"      )) throw new ArgumentException(_errorMessage + ": baseDomain");
            if (!cols.Any(c => c.ColumnName == "originAttributes")) throw new ArgumentException(_errorMessage + ": originAttributes");
            if (!cols.Any(c => c.ColumnName == "name"            )) throw new ArgumentException(_errorMessage + ": name");
            if (!cols.Any(c => c.ColumnName == "value"           )) throw new ArgumentException(_errorMessage + ": value");
            if (!cols.Any(c => c.ColumnName == "host"            )) throw new ArgumentException(_errorMessage + ": host");
            if (!cols.Any(c => c.ColumnName == "path"            )) throw new ArgumentException(_errorMessage + ": path");
            if (!cols.Any(c => c.ColumnName == "expiry"          )) throw new ArgumentException(_errorMessage + ": expiry");
            if (!cols.Any(c => c.ColumnName == "lastAccessed"    )) throw new ArgumentException(_errorMessage + ": lastAccessed");
            if (!cols.Any(c => c.ColumnName == "creationTime"    )) throw new ArgumentException(_errorMessage + ": creationTime");
            if (!cols.Any(c => c.ColumnName == "isSecure"        )) throw new ArgumentException(_errorMessage + ": isSecure");
            if (!cols.Any(c => c.ColumnName == "isHttpOnly"      )) throw new ArgumentException(_errorMessage + ": isHttpOnly");
            if (!cols.Any(c => c.ColumnName == "inBrowserElement")) throw new ArgumentException(_errorMessage + ": inBrowserElement");
            if (!cols.Any(c => c.ColumnName == "sameSite"        )) throw new ArgumentException(_errorMessage + ": sameSite");

            var rows = chromeCookieTable.Rows.OfType<DataRow>().ToArray();
            if (!rows.Any(r => (r["host"].ToString() == ".nicovideo.jp") && (r["name"].ToString() == "user_session")))
            {
                throw new ArgumentException(_errorMessage);
            }

            var row = rows.Where(r => (r["host"].ToString() == ".nicovideo.jp") && (r["name"].ToString() == "user_session"))
                          .FirstOrDefault();

            var entity = new Entity(row);
            var cookie = new Cookie("user_session", entity.Value, entity.Path, entity.HostKey);
            this.Add(cookie);
        }

        private class Entity
        {
            public readonly long ID;
            public readonly string BaseDomain;
            public readonly string OriginAttributes;
            public readonly string Name;
            public readonly string Value;
            public readonly string HostKey;
            public readonly string Path;
            public readonly long Expiry;
            public readonly long LastAccessed;
            public readonly long CreationTime;
            public readonly long IsSecure;
            public readonly long IsHttpOnly;
            public readonly long InBrowserElement;
            public readonly long SameSite;

            public Entity (
                DataRow firefoxCookieRow
            )
            {
                long.TryParse(firefoxCookieRow["id"].ToString()              , out this.ID);
                long.TryParse(firefoxCookieRow["expiry"].ToString()          , out this.Expiry);
                long.TryParse(firefoxCookieRow["lastAccessed"].ToString()    , out this.LastAccessed);
                long.TryParse(firefoxCookieRow["creationTime"].ToString()    , out this.CreationTime);
                long.TryParse(firefoxCookieRow["isSecure"].ToString()        , out this.IsSecure);
                long.TryParse(firefoxCookieRow["isHttpOnly"].ToString()      , out this.IsHttpOnly);
                long.TryParse(firefoxCookieRow["inBrowserElement"].ToString(), out this.InBrowserElement);
                long.TryParse(firefoxCookieRow["sameSite"].ToString()        , out this.SameSite);
                BaseDomain       = firefoxCookieRow["baseDomain"].ToString();
                OriginAttributes = firefoxCookieRow["originAttributes"].ToString();
                Name             = firefoxCookieRow["name"].ToString();
                Value            = firefoxCookieRow["value"].ToString();
                HostKey          = firefoxCookieRow["host"].ToString();
                Path             = firefoxCookieRow["path"].ToString();
            }
        }
    }
}
