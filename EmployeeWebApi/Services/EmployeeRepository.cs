using EmployeeWebApi.Entities;
using EmployeeWebApi.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EmployeeWebApi.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _context;

        public EmployeeRepository(EmployeeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Employee?> GetEmployeeAsync(Guid id)
        {
            return await _context.Employees.Where(e => e.Id == id).Include(e => e.Role).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _context.Employees.Include(e => e.Role).OrderBy(e => e.LastName).ToListAsync();
        }
    }
}
