using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeWebApi.Entities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } 
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public DateTime EmploymentDate { get; set; }
        [Required]
        public string HomeAddress { get; set; } 
        [Required]
        public decimal CurrentSalary { get; set; }
        public Guid? BossId { get; set; }
        [ForeignKey("RoleId")]
        [Required]
        public Guid RoleId { get; set; }
        [Required]
        public Role? Role { get; set; }

        public Employee(string firstName, string lastName, DateTime birthDate, DateTime employmentDate, string homeAddress, decimal currentSalary, Guid? bossId, Guid roleId)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            EmploymentDate = employmentDate;
            HomeAddress = homeAddress;
            CurrentSalary = currentSalary;
            BossId = bossId;
            RoleId = roleId;
        }
    }
}
