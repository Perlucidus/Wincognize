using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UrlHistoryLibrary;
using Wincognize.Data;

namespace Wincognize.Tracking
{
    public class BrowsingHistoryTracker : Tracker
    {
        private const int DelayInterval = 10000;
        private Task m_task;
        private CancellationTokenSource m_cts;
        private DateTime m_lastUpdate;

        public BrowsingHistoryTracker()
        {
            m_cts = new CancellationTokenSource();
            m_task = new Task(Update, m_cts.Token);
            m_lastUpdate = DateTime.UtcNow;
        }

        #region Tracker Implementation

        protected override void DisposeManaged() { }

        protected override void DisposeUnmanaged() { }

        protected override void PDisable()
        {
            m_cts.Cancel();
        }

        protected override void PEnable()
        {
            m_task.Start();
        }

        #endregion

        private async void Update(object obj)
        {
            CancellationToken cts = (CancellationToken)obj;
            while (!cts.IsCancellationRequested)
            {
                //Update Google Chrome browsing history
                string google = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\History";
                string fileName = "chromehistory.sqlite";
                string path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\Wincognize\{fileName}";
                File.Copy(google, path, true);
                using (PreparedStatement ps = new Database($"Data Source={path};Version=3;Compress=true;").PrepareStatement($"SELECT * FROM urls WHERE last_visit_time > {m_lastUpdate.ToFileTimeUtc() / 10} ORDER BY last_visit_time DESC"))
                using (ResultSet rs = ps.ExecuteQuery())
                    //url,title,visit_count,last_visit_time
                    while (rs.NextRow())
                        Database.Main.ExecuteNonQuery($"INSERT INTO History (url, visit_time) VALUES ('{StringUtilities.ToLiteral(rs.Get<string>("url"))}', '{rs.Get<ulong>("last_visit_time")*10}')");
                //Update Internet Explorer browsing history
                UrlHistoryWrapperClass urlhistory = new UrlHistoryWrapperClass();
                foreach (STATURL staturl in urlhistory)
                    if (staturl.LastVisited > m_lastUpdate && staturl.URL.StartsWith("http"))
                        Database.Main.ExecuteNonQuery($"INSERT INTO History (url, visit_time) VALUES ('{StringUtilities.ToLiteral(staturl.URL)}', '{staturl.LastVisited.ToFileTimeUtc()}')");
                m_lastUpdate = DateTime.UtcNow;
                await Task.Delay(DelayInterval);
            }
        }
    }
}
