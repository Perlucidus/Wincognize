using System;
using System.Windows.Forms;
using Wincognize.Data;

namespace Wincognize
{
    public class Context : ApplicationContext
    {
        public Context()
        {
            Application.ApplicationExit += OnApplicationExit;
            /*using (DataConnection connection = Database.Instance.CreateConnection())
            {
                //connection.ExecuteNonQuery("create table test1(s varchar(20))");
                //connection.ExecuteNonQuery("insert into test1 (s) values ('a')");
                using (ResultSet rs = connection.ExecuteQuery("select * from test1"))
                    while (rs.NextRow())
                        Console.WriteLine(rs.Get<string>("s"));
            }*/
        }

        private void OnApplicationExit(Object sender, EventArgs e)
        {
            Console.WriteLine("Application Exit");
        }
    }
}
