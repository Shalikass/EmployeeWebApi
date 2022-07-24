using EmployeeWebApi.Entities;

namespace EmployeeWebApi.Services
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync(
            string? searchQuery, Guid? bossId, DateTime? birthDateFrom, DateTime? birthDateTo);

        Task<Employee?> GetEmployeeAsync(Guid id);
        Task<Employee?> GetCEOAsync();
        Task<bool> EmployeeExistsAsync(Guid? id);
        Task<bool> SaveChangesAsync();
        void AddEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
        Task<Role?> GetRoleAsync(Guid id);
        Task<int> GetRoleEmployeeCountAsync(Guid id);

    }
}
