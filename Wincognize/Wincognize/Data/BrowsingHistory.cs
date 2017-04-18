using System.ComponentModel.DataAnnotations.Schema;

namespace Wincognize.Data
{
    [Table("BrowsingHistory")]
    public partial class BrowsingHistory
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public long VisitTime { get; set; }
    }
}
