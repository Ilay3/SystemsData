using System;
using System.Collections.ObjectModel;

namespace SystemsData.ViewModels
{
    public class SystemEntityViewModel
    {
        public int SystemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public ObservableCollection<BaseComponentViewModel> BaseComponents { get; set; } = new ObservableCollection<BaseComponentViewModel>();
    }
}
