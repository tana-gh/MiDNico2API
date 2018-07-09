namespace MiDCookieGetter
{
    using System;
    using System.Data;
    using System.IO;
    using Microsoft.Data.Sqlite;

    public class FireFoxCookieGetter : ICookieGetter
    {
        public static string DefaultWindowsPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Mozilla\Firefox\Profiles\frnx4wzs.default\cookies.sqlite");
        public static string DefaultMacPath     => throw new NotImplementedException(@"現在、Mac版は工事中です。");

        private string _dbPath;
        
        private string _hostKey  = null;
        private string _name     = null;

        private string _template => (_hostKey == null && _name == null) ? throw new Exception("SQLに必要なパラメータが設定されていません。")
                                  : (_hostKey != null && _name == null) ? @"SELECT * FROM moz_cookies WHERE host = @host_key"
                                  : (_hostKey == null && _name != null) ? @"SELECT * FROM moz_cookies WHERE name = @name"
                                                                        : @"SELECT * FROM moz_cookies WHERE host = @host_key AND name = @name";

        private Func<SqliteCommand, Func<string, Func<string, SqliteParameter>>>
                       _ParamCreator => cmd => key => val =>
        {
            #region 処理
            var param = cmd.CreateParameter();
            param.ParameterName = key;
            param.SqliteType = SqliteType.Text;
            param.Direction = ParameterDirection.Input;
            param.Value = val;
            return param;
            #endregion
        };

        private FireFoxCookieGetter() { }

        public static FireFoxCookieGetter Prepare(string firefoxCookieDBPath)
        {
            var getter = new FireFoxCookieGetter();
            getter._dbPath = firefoxCookieDBPath;
            return getter;
        }

        public ICookieGetter WithHostKey(string hostKey)
        {
            _hostKey = hostKey;
            return this;
        }

        public ICookieGetter WithName(string name)
        {
            _name = name;
            return this;
        }

        public DataTable Do()
        {
            if (!File.Exists(_dbPath)) throw new FileNotFoundException(@"Cookieファイルが見つかりませんでした。");
            if (_hostKey == null && _name == null) throw new ArgumentNullException(@"SQLに必要なパラメータが設定されていません。");

            var builder = new SqliteConnectionStringBuilder() { DataSource = _dbPath };
            using (var conn = new SqliteConnection(builder.ConnectionString))
            {
                conn.Open();
                var query = _template;
                var cmd = new SqliteCommand(query, conn);

                var creator = _ParamCreator(cmd);
                if (_hostKey != null) cmd.Parameters.Add(creator("@host_key")(_hostKey));
                if (_name != null) cmd.Parameters.Add(creator("@name")(_name));

                var reader = cmd.ExecuteReader();
                var dataset = new DataSet();
                dataset.EnforceConstraints = false;

                try
                {
                    dataset.Load(reader, LoadOption.OverwriteChanges, "cookie");
                }
                catch { throw; }

                return dataset.Tables[0];
            }
        }
    }
}
