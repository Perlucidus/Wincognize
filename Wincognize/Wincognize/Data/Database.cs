using System.Data.Entity;

namespace Wincognize.Data
{
    public class DataContext : DbContext
    {
        public static DataContext Main;

        public DbSet<Keyboard> Keyboard { get; set; }
        public DbSet<Mouse> Mouse { get; set; }
        public DbSet<BrowsingHistory> BrowsingHistory { get; set; }

        static DataContext()
        {
            Main = new DataContext();
        }

        private DataContext() : base("Data Source=(localdb)\\MSSQLLocalDB;Database=Wincognize;Integrated Security=SSPI;") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Set custom initializer
            Database.SetInitializer<DataContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
