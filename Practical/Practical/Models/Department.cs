using System.ComponentModel.DataAnnotations;

namespace Practical.Models
{
    public class Department
    {
        [Key]
        public char DepartmentId { get; set; }

        public string Name { get; set; }

        public decimal AnnualBudget { get; set; }
    }
}
