using System.ComponentModel.DataAnnotations.Schema;

namespace Wincognize.Data
{
    [Table("Mouse")]
    public partial class Mouse
    {
        public int Id { get; set; }
        public int Action { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Data { get; set; }
        public int Flags { get; set; }
        public int Timestamp { get; set; }
        public int ExtraInfo { get; set; }
    }
}
