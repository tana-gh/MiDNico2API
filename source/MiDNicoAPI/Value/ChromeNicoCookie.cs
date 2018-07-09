namespace MiDNicoAPI
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Google ChromeのCookie DBから取得した情報から,
    /// ニコニコ通信用のCookie情報を生成し, 管理するクラス.
    /// </summary>
    public class ChromeNicoCookie : CookieContainer
    {
        private static readonly string _errorMessage = "Google ChromeのCookie情報を指定してください。";

        public ChromeNicoCookie (
            in DataTable chromeCookieTable
        )
        {
            if (chromeCookieTable            == null) throw new ArgumentException("Cookie情報にnullは指定できません。");
            if (chromeCookieTable.Rows.Count <= 0   ) throw new ArgumentException("Cookie情報に空データは指定できません。");

            var cols = chromeCookieTable.Columns.OfType<DataColumn>().ToArray();
            if (!cols.Any(c => c.ColumnName == "creation_utc"   )) throw new ArgumentException(_errorMessage + ": creation_utc");
            if (!cols.Any(c => c.ColumnName == "host_key"       )) throw new ArgumentException(_errorMessage + ": host_key");
            if (!cols.Any(c => c.ColumnName == "name"           )) throw new ArgumentException(_errorMessage + ": name");
            if (!cols.Any(c => c.ColumnName == "value"          )) throw new ArgumentException(_errorMessage + ": value");
            if (!cols.Any(c => c.ColumnName == "path"           )) throw new ArgumentException(_errorMessage + ": path");
            if (!cols.Any(c => c.ColumnName == "expires_utc"    )) throw new ArgumentException(_errorMessage + ": expires_utc");
            if (!cols.Any(c => c.ColumnName == "is_secure"      )) throw new ArgumentException(_errorMessage + ": is_secure");
            if (!cols.Any(c => c.ColumnName == "is_httponly"    )) throw new ArgumentException(_errorMessage + ": is_httponly");
            if (!cols.Any(c => c.ColumnName == "last_access_utc")) throw new ArgumentException(_errorMessage + ": last_access_utc");
            if (!cols.Any(c => c.ColumnName == "has_expires"    )) throw new ArgumentException(_errorMessage + ": has_expires");
            if (!cols.Any(c => c.ColumnName == "is_persistent"  )) throw new ArgumentException(_errorMessage + ": is_persistent");
            if (!cols.Any(c => c.ColumnName == "priority"       )) throw new ArgumentException(_errorMessage + ": priority");
            if (!cols.Any(c => c.ColumnName == "encrypted_value")) throw new ArgumentException(_errorMessage + ": encrypted_value");
            if (!cols.Any(c => c.ColumnName == "firstpartyonly" )) throw new ArgumentException(_errorMessage + ": firstpartyonly");

            var rows = chromeCookieTable.Rows.OfType<DataRow>().ToArray();
            if (!rows.Any(r => (r["host_key"].ToString() == ".nicovideo.jp") && (r["name"].ToString() == "user_session") ))
            {
                throw new ArgumentException(_errorMessage);
            }

            var row = rows.Where(r => (r["host_key"].ToString() == ".nicovideo.jp") && (r["name"].ToString() == "user_session"))
                          .FirstOrDefault();

            var entity = new Entity(row);
            var cookie = new Cookie("user_session", entity.EncryptedValue, entity.Path, entity.HostKey);
            this.Add(cookie);
        }

        private class Entity
        {
            public readonly long   CreationUtc;
            public readonly string HostKey;
            public readonly string Name;
            public readonly string Value;
            public readonly string Path;
            public readonly long   ExpiresUtc;
            public readonly long   Secure;
            public readonly long   HttpOnly;
            public readonly long   LastAccessUtc;
            public readonly long   HasExpires;
            public readonly long   Persistent;
            public readonly long   Priority;
            public          string EncryptedValue 
            {
                get => _encryptedValue != null
                        ? Encoding.UTF8.GetString(ProtectedData.Unprotect(_encryptedValue, null, DataProtectionScope.CurrentUser))
                        : string.Empty;
            }
            public readonly long   FirstPartyOnly;

            private readonly byte[] _encryptedValue;

            public Entity (
                DataRow chromeCookieRow
            )
            {
                long.TryParse(chromeCookieRow["creation_utc"].ToString(), out this.CreationUtc);
                long.TryParse(chromeCookieRow["expires_utc"].ToString(), out this.ExpiresUtc);
                long.TryParse(chromeCookieRow["is_secure"].ToString(), out this.Secure);
                long.TryParse(chromeCookieRow["is_httponly"].ToString(), out this.HttpOnly);
                long.TryParse(chromeCookieRow["last_access_utc"].ToString(), out this.LastAccessUtc);
                long.TryParse(chromeCookieRow["has_expires"].ToString(), out this.HasExpires);
                long.TryParse(chromeCookieRow["is_persistent"].ToString(), out this.Persistent);
                long.TryParse(chromeCookieRow["priority"].ToString(), out this.Priority);
                long.TryParse(chromeCookieRow["firstpartyonly"].ToString(), out this.FirstPartyOnly);
                HostKey = chromeCookieRow["host_key"].ToString();
                Name    = chromeCookieRow["name"].ToString();
                Value   = chromeCookieRow["value"].ToString();
                Path    = chromeCookieRow["path"].ToString();
                _encryptedValue = (byte[])chromeCookieRow["encrypted_value"];
            }

        }
    }
}
