using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wincognize.Data
{
    public class DataConnection : IDisposable
    {
        private SQLiteConnection m_connection;

        public DataConnection(string connection)
        {
            m_connection = new SQLiteConnection(connection);
            m_connection.Open();
        }

        ~DataConnection()
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
                m_connection.Close();
        }

        public int ExecuteNonQuery(string command)
        {
            return new SQLiteCommand(command, m_connection).ExecuteNonQuery();
        }

        public ResultSet ExecuteQuery(string query)
        {
            return new ResultSet(new SQLiteCommand(query, m_connection).ExecuteReader());
        }
    }
}
