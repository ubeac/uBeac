using uBeac.Common;

namespace uBeac.IoT.Models
{
    public class File : BaseEntity
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
    }
}
