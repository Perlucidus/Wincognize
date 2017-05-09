using System;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UrlHistoryLibrary;
using Wincognize.Data;

namespace Wincognize.Tracking
{
    public class BrowsingHistoryTracker : Tracker
    {
        private const int DelayInterval = 10000; //Task execute interval
        private Task m_task;
        private CancellationTokenSource m_cts;
        private DateTime m_lastUpdate; //Last database update

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
                //Locate google chrome browsing history database
                string google = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\History";
                string fileName = "chromehistory.sqlite";
                string path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\Wincognize\{fileName}";
                File.Copy(google, path, true); //Copy to grant reading access
                lock (DataContext.Main)
                {
                    //Update database for google chrome browsing history
                    using (var con = new SQLiteConnection($"Data Source={path};Version=3;Compress=true;"))
                    {
                        con.Open();
                        using (var cmd = new SQLiteCommand($"SELECT * FROM urls WHERE last_visit_time > {m_lastUpdate.ToFileTimeUtc() / 10} ORDER BY last_visit_time DESC", con))
                        using (var reader = cmd.ExecuteReader())
                            while (reader.Read())
                                DataContext.Main.BrowsingHistory.Add(new BrowsingHistory { URL = (string)reader["url"], VisitTime = (long)reader["last_visit_time"] * 10 });
                    }
                    //Update database for internet explorer browsing history
                    UrlHistoryWrapperClass urlhistory = new UrlHistoryWrapperClass();
                    foreach (STATURL staturl in urlhistory)
                        if (staturl.LastVisited > m_lastUpdate && staturl.URL.StartsWith("http"))
                            DataContext.Main.BrowsingHistory.Add(new BrowsingHistory { URL = staturl.URL, VisitTime = staturl.LastVisited.ToFileTimeUtc() });
                    DataContext.Main.SaveChanges();
                }
                m_lastUpdate = DateTime.UtcNow;
                await Task.Delay(DelayInterval);
            }
        }
    }
}
