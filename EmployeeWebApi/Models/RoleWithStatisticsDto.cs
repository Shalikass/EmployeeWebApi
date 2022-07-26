namespace EmployeeWebApi.Models
{
    public class RoleWithStatisticsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public RoleStatisticsDto Statistics { get; set; } = new RoleStatisticsDto();
    }
}
