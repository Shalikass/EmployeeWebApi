namespace EmployeeWebApi.Entities
{
    public class RoleStatistics
    {
        public int EmployeeCount { get; set; }
        public decimal AverageSalary { get; set; }
        public RoleStatistics(int employeeCount, decimal averageSalary)
        {
            EmployeeCount = employeeCount;
            AverageSalary = averageSalary;
        }
    }
}
