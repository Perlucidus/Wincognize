using System.ComponentModel.DataAnnotations.Schema;

namespace Wincognize.Data
{
    [Table("Keyboard")]
    public partial class Keyboard
    {
        public int Id { get; set; }
        public int Action { get; set; }
        public int VkCode { get; set; }
        public int HwsCode { get; set; }
        public int Flags { get; set; }
        public int Timestamp { get; set; }
        public int ExtraInfo { get; set; }
    }
}
