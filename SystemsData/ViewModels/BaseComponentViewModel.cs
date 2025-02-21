using System;
using System.Collections.ObjectModel;

namespace SystemsData.ViewModels
{
    public class BaseComponentViewModel
    {
        public int ComponentId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = "проверено";
        public string? Details { get; set; }
        public string? ReplacementInfo { get; set; }
        public byte[]? FileAttachment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public ObservableCollection<ComponentPartViewModel> ComponentParts { get; set; } = new ObservableCollection<ComponentPartViewModel>();
    }
}
