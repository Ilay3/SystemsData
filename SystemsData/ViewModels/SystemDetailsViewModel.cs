using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemsData.Models;

namespace SystemsData.ViewModels
{
    public class SystemDetailsViewModel
    {
        public int SystemId { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByName { get; set; } = "";
        public System.Collections.Generic.List<BaseComponent> BaseComponents { get; set; } = new System.Collections.Generic.List<BaseComponent>();
    }
}
