using System;
using System.Data;
using System.Data.SQLite;

namespace Wincognize.Data
{
    public class PreparedStatement : IDisposable
    {
        private SQLiteCommand m_command;

        public PreparedStatement(SQLiteConnection connection, string ps)
        {
            m_command = new SQLiteCommand(ps, connection);
        }

        ~PreparedStatement()
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
                m_command.Dispose();
        }

        public object this[int index]
        {
            get
            {
                return m_command.Parameters[index].Value;
            }
            set
            {
                if (m_command.Parameters.Count <= index)
                    m_command.Parameters.Add(new SQLiteParameter());
                m_command.Parameters[index].Value = value;
            }
        }

        public object this[string name]
        {
            get
            {
                return m_command.Parameters[name].Value;
            }
            set
            {
                if (!m_command.Parameters.Contains(name))
                    m_command.Parameters.Add(new SQLiteParameter(name));
                m_command.Parameters[name].Value = value;
            }
        }

        public int ExecuteNonQuery()
        {
            return m_command.ExecuteNonQuery();
        }

        public ResultSet ExecuteQuery()
        {
            return new ResultSet(m_command.ExecuteReader());
        }
    }
}
