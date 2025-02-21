using System.ComponentModel.DataAnnotations;

namespace SystemsData.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
    }
}
