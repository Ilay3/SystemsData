using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SystemsData.Models
{
    public class BaseComponent
    {
        [Key]
        public int ComponentId { get; set; }
        public int? SystemId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Details { get; set; }
        public string Status { get; set; } = "проверено";
        public string? ReplacementInfo { get; set; }
        public byte[]? FileAttachment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        public SystemEntity? SystemEntity { get; set; }
        public ICollection<ComponentPart> ComponentParts { get; set; } = new List<ComponentPart>();
    }
}
