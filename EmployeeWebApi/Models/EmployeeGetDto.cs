namespace EmployeeWebApi.Models
{
    public class EmployeeGetDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public DateTime EmploymentDate { get; set; }
        public Guid? BossId { get; set; }
        public decimal CurrentSalary { get; set; }
        public RoleDto Role { get; set; } = new RoleDto();
        public string HomeAddress { get; set; } = string.Empty;
    }
}
