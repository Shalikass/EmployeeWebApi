using EmployeeWebApi.Entities;
using EmployeeWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWebApi.Validators
{
    public static class CustomValidations
    {
        public static async Task<ObjectResult?> CheckCreateConstraints(Employee employee, IEmployeeRepository employeeRepository)
        {
            var role = await employeeRepository.GetRoleAsync(employee.RoleId);
            if (role == null)
            {
                return new NotFoundObjectResult($"Role with id \"{employee.RoleId}\" does not exist.");
            }

            if (role.Name == Constants.RoleNameCEO)
            {
                if (await employeeRepository.GetRoleEmployeeCountAsync(employee.RoleId) > 0)
                {
                    return new UnprocessableEntityObjectResult($"Only one employee can have {Constants.RoleNameCEO} role.");
                }
                if (employee.BossId != null)
                {
                    return new UnprocessableEntityObjectResult($"Employee with {Constants.RoleNameCEO} role cannot have a boss.");
                }
            }
            else
            {
                return await CheckBossExists(employee, employeeRepository);
            }
            return null;
        }


        public static async Task<ObjectResult?> CheckUpdateConstraints(Employee employee, IEmployeeRepository employeeRepository)
        {
            var role = await employeeRepository.GetRoleAsync(employee.RoleId);
            if (role == null)
            {
                return new NotFoundObjectResult($"Role with id \"{employee.RoleId}\" does not exist.");
            }

            var currentCEO = await employeeRepository.GetCEOAsync();
            if (role.Name == Constants.RoleNameCEO)
            {
                if (currentCEO != null && employee.Id != currentCEO.Id)
                {
                    return new UnprocessableEntityObjectResult($"Only one employee can have {Constants.RoleNameCEO} role.");
                }
                if (employee.BossId != null)
                {
                    return new UnprocessableEntityObjectResult($"Employee with {Constants.RoleNameCEO} role cannot have a boss.");
                }
            }
            else
            {
                return await CheckBossExists(employee, employeeRepository);
            }
            return null;
        }

        public static async Task<ObjectResult?> CheckBossExists(Employee employee, IEmployeeRepository employeeRepository)
        {
            if (employee.BossId == null)
            {
                return new NotFoundObjectResult($"Boss id for role for this role cannot be null.");
            }
            else if (!await employeeRepository.EmployeeExistsAsync(employee.BossId))
            {
                return new NotFoundObjectResult($"Employee with id \"{employee.BossId}\" does not exist.");
            }
            return null;
        }
    }
}
