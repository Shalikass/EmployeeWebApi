using EmployeeWebApi.Entities;

namespace EmployeeWebApi.Services
{
    public interface IEmployeeRepository
    {
        Task<(IEnumerable<Employee>, PaginationMetadata)> GetEmployeesAsync(
            string? searchQuery, Guid? bossId, DateTime? birthDateFrom, DateTime? birthDateTo, int pageNumber, int pageSize);
        Task<Employee?> GetEmployeeAsync(Guid id);
        Task<Employee?> GetCEOAsync();
        Task<bool> EmployeeExistsAsync(Guid? id);
        Task<bool> SaveChangesAsync();
        void AddEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
        Task<(IEnumerable<Role>, PaginationMetadata)> GetRolesAsync(string? searchQuery, int pageNumber, int pageSize);
        Task<Role?> GetRoleAsync(Guid id);
        Task<int> GetRoleEmployeeCountAsync(Guid id);
        Task<decimal> GetRoleEmployeeCurrentSalarySumAsync(Guid roleId);

    }
}
