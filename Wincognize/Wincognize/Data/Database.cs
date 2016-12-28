using System;
using System.Data.SQLite;
using System.IO;

namespace Wincognize.Data
{
    public class Database
    {
        public static Database Instance;

        private const string FilePath = @"{0}\Wincognize";
        private const string FileName = "data.sqlite";
        private const int SqliteVersion = 3;

        private string m_connection;

        static Database()
        {
            Instance = new Database();
        }

        private Database()
        {
            string dir = string.Format(FilePath, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            string path = $@"{dir}\{FileName}";
            Directory.CreateDirectory(dir);
            if (!File.Exists(path))
                SQLiteConnection.CreateFile(path);
            m_connection = $"Data Source={path};Version={SqliteVersion};";
        }

        public DataConnection CreateConnection()
        {
            return new DataConnection(m_connection);
        }
    }
}
