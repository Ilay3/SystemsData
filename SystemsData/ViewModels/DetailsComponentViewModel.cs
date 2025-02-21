using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsData.ViewModels
{
    public class DetailsComponentViewModel
    {
        public int ComponentId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public string ReplacementInfo { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public byte[] FileAttachment { get; set; }
        public string FileAttachmentLength { get; set; }
    }

}
