using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SystemsData.Models
{
    public class SystemEntity
    {
        [Key]
        public int SystemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        public ICollection<BaseComponent> BaseComponents { get; set; } = new List<BaseComponent>();
    }
}
