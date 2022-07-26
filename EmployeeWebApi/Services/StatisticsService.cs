using EmployeeWebApi.Entities;

namespace EmployeeWebApi.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public StatisticsService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }
        public async Task<RoleStatistics> GetRoleStatisticsAsync(Role role)
        {
            var employeeCount = await _employeeRepository.GetRoleEmployeeCountAsync(role.Id);
            var salarySum = await _employeeRepository.GetRoleEmployeeCurrentSalarySum(role.Id);
            return new RoleStatistics(
                employeeCount,
                employeeCount == 0 ? 0 : (salarySum / employeeCount));
        }
    }
}
