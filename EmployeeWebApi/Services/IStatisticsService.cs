using EmployeeWebApi.Entities;

namespace EmployeeWebApi.Services
{
    public interface IStatisticsService
    {
        Task<RoleStatistics> GetRoleStatisticsAsync(Role role);
    }
}