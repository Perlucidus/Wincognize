using System;
using System.Data.SQLite;

namespace Wincognize.Data
{
    public class ResultSet : IDisposable
    {
        private SQLiteDataReader m_reader;

        public ResultSet(SQLiteDataReader reader)
        {
            m_reader = reader;
        }

        ~ResultSet()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                m_reader.Close();
        }

        public bool NextRow()
        {
            return m_reader.Read();
        }

        public T Get<T>(string column)
        {
            return (T)(Convert.ChangeType(m_reader[column], typeof(T)));
        }
    }
}
