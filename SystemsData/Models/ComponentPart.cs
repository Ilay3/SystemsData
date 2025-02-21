using System;
using System.ComponentModel.DataAnnotations;

namespace SystemsData.Models
{
    public class ComponentPart
    {
        [Key]
        public int PartId { get; set; }
        public int ParentComponentId { get; set; }
        public string PartType { get; set; } = string.Empty; // "board", "sensor" и т.д.
        public string PartNumber { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
        public string? AdditionalDetails { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        public BaseComponent ParentComponent { get; set; } = null!;
    }
}
