using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsData.ViewModels
{
    public class FaultyComponentViewModel
    {
        public int ComponentId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByName { get; set; }
    }
}
