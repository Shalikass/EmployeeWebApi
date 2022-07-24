using System.ComponentModel.DataAnnotations;

namespace EmployeeWebApi.Models
{
    public class EmployeeUpdateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public DateTime EmploymentDate { get; set; }
        public Guid? BossId { get; set; }
        public decimal CurrentSalary { get; set; }
        public Guid RoleId { get; set; }
        public string HomeAddress { get; set; } = string.Empty;
    }
}
