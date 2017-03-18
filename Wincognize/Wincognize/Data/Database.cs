using System;
using System.Data.SQLite;
using System.IO;

namespace Wincognize.Data
{
    public class Database
    {
        public static Database Main;

        private const string FilePath = @"{0}\Wincognize";
        private const string FileName = "data.sqlite";
        private const int SqliteVersion = 3;

        private SQLiteConnection m_connection;

        static Database()
        {
            //Initialize main database
            string dir = string.Format(FilePath, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            string path = $@"{dir}\{FileName}";
            Directory.CreateDirectory(dir);
            if (!File.Exists(path))
                SQLiteConnection.CreateFile(path);
            Main = new Database($"Data Source={path};Version={SqliteVersion};");
        }

        public Database(string conStr)
        {
            m_connection = new SQLiteConnection(conStr);
            m_connection.Open();
        }

        public PreparedStatement PrepareStatement(string ps)
        {
            return new PreparedStatement(m_connection, ps);
        }

        public void ExecuteNonQuery(string sql)
        {
            using (PreparedStatement ps = PrepareStatement(sql))
                ps.ExecuteNonQuery();
        }
    }
}
