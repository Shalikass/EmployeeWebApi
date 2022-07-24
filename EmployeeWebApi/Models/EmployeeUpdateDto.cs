using System.ComponentModel.DataAnnotations;

namespace EmployeeWebApi.Models
{
    public class EmployeeUpdateDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public DateTime EmploymentDate { get; set; }
        [Required]
        public Guid? BossId { get; set; }
        [Required]
        public decimal CurrentSalary { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        [Required]
        public string HomeAddress { get; set; } = string.Empty;
    }
}
