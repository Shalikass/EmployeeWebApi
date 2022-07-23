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

        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        public async Task<bool> EmployeeExistsAsync(Guid id)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id);
        }

        public async Task<Employee?> GetEmployeeAsync(Guid id)
        {
            return await _context.Employees.Where(e => e.Id == id).Include(e => e.Role).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(
            string? searchQuery, Guid? bossId, DateTime? birthDateFrom, DateTime? birthDateTo)
        {
            var collection = _context.Employees as IQueryable<Employee>;

            if (bossId != null)
                collection = collection.Where(e => e.BossId == bossId);
            if (birthDateFrom != null)
                collection = collection.Where(e => e.BirthDate >= birthDateFrom);
            if (birthDateTo != null)
                collection = collection.Where(e => e.BirthDate <= birthDateTo);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(e => e.FirstName.Contains(searchQuery)
                    || e.LastName.Contains(searchQuery));
            }
            return await collection.Include(e => e.Role).OrderBy(e => e.LastName).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
