﻿using EmployeeWebApi.Entities;
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

        public async Task<bool> EmployeeExistsAsync(Guid? id)
        {
            if (id == null) return false;
            return await _context.Employees.AnyAsync(e => e.Id == id);
        }

        public async Task<Employee?> GetCEOAsync()
        {
            return await _context.Employees.Include(e => e.Role).FirstOrDefaultAsync(e => e.Role != null && e.Role.Name == Constants.RoleNameCEO);
        }

        public async Task<Employee?> GetEmployeeAsync(Guid id)
        {
            return await _context.Employees.Where(e => e.Id == id).Include(e => e.Role).FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<Employee>, PaginationMetadata)> GetEmployeesAsync(string? searchQuery, Guid? bossId, DateTime? birthDateFrom, DateTime? birthDateTo, int pageNumber, int pageSize)
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
                collection = collection.Where(e => e.FirstName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                    || e.LastName.Contains(searchQuery));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var result = await collection.Include(e => e.Role)
                                                     .OrderBy(e => e.LastName)
                                                     .ThenBy(e => e.FirstName)
                                                     .Skip(pageSize * (pageNumber - 1))
                                                     .Take(pageSize)
                                                     .ToListAsync();

            return (result, paginationMetadata);
        }

        public async Task<Role?> GetRoleAsync(Guid id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<int> GetRoleEmployeeCountAsync(Guid id)
        {
            return await _context.Employees.Include(e => e.Role).CountAsync(e => e.Role != null && e.Role.Id == id);
        }

        public async Task<decimal> GetRoleEmployeeCurrentSalarySumAsync(Guid roleId)
        {
            return await _context.Employees.Include(e => e.Role)
                                           .Where(e => e.Role != null && e.Role.Id == roleId)
                                           .SumAsync(e => e.CurrentSalary);
        }

        public async Task<(IEnumerable<Role>, PaginationMetadata)> GetRolesAsync(string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _context.Roles as IQueryable<Role>;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(r => r.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                    || (r.Description != null && r.Description.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var result = await collection.OrderBy(r => r.Name)
                                         .Skip(pageSize * (pageNumber - 1))
                                         .Take(pageSize)
                                         .ToListAsync();

            return (result, paginationMetadata);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
