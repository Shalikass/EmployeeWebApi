using EmployeeWebApi.Entities;

namespace EmployeeWebApi.Services
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee?> GetEmployeeAsync(Guid id);
    }
}
