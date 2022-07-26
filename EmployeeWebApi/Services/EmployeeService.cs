using EmployeeWebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWebApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        public readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        private async Task<IResult> CheckBossExists_Async(Employee employee)
        {
            if (employee.BossId == null)
            {
                return new Result(ResultCode.CannotProcess, $"Boss id for this role cannot be null.");
            }
            else if (!await _employeeRepository.EmployeeExistsAsync(employee.BossId))
            {
                return new Result(ResultCode.NotFound, $"Employee with id \"{employee.BossId}\" does not exist.");
            }
            return new Result(ResultCode.Success);
        }

        public async Task<IResult> CreateEmployeeAsync(Employee employee)
        {
            var role = await _employeeRepository.GetRoleAsync(employee.RoleId);
            if (role == null)
            {
                return new Result(ResultCode.NotFound, $"Role with id \"{employee.RoleId}\" does not exist.");
            }

            if (role.Name == Constants.RoleNameCEO)
            {
                if (await _employeeRepository.GetRoleEmployeeCountAsync(employee.RoleId) > 0)
                {
                    return new Result(ResultCode.CannotProcess, $"Only one employee can have {Constants.RoleNameCEO} role.");
                }
                if (employee.BossId != null)
                {
                    return new Result(ResultCode.CannotProcess, $"Employee with {Constants.RoleNameCEO} role cannot have a boss.");
                }
            }
            else
            {
                var result = await CheckBossExists_Async(employee);
                if (result.ResultCode != ResultCode.Success)
                    return result;
            }
            _employeeRepository.AddEmployee(employee);
            await _employeeRepository.SaveChangesAsync();
            return new Result(ResultCode.Success);
        }

        public async Task<IResult> UpdateEmployeeAsync(Employee employee)
        {
            var role = await _employeeRepository.GetRoleAsync(employee.RoleId);
            if (role == null)
            {
                return new Result(ResultCode.NotFound, $"Role with id \"{employee.RoleId}\" does not exist.");
            }

            var currentCEO = await _employeeRepository.GetCEOAsync();
            if (role.Name == Constants.RoleNameCEO)
            {
                if (currentCEO != null && employee.Id != currentCEO.Id)
                {
                    return new Result(ResultCode.CannotProcess, $"Only one employee can have {Constants.RoleNameCEO} role.");
                }
                if (employee.BossId != null)
                {
                    return new Result(ResultCode.CannotProcess, $"Employee with {Constants.RoleNameCEO} role cannot have a boss.");
                }
            }
            else
            {
                var result = await CheckBossExists_Async(employee);
                if (result.ResultCode != ResultCode.Success)
                    return result;
            }

            await _employeeRepository.SaveChangesAsync();
            return new Result(ResultCode.Success);
        }

        public class Result : IResult
        {
            private readonly ResultCode _resultCode;
            private readonly string? _message;

            public ResultCode ResultCode => _resultCode;
            public string? Message => _message;

            public Result(ResultCode code) : this(code, null)
            {
            }
            public Result(ResultCode code, string? message)
            {
                _resultCode = code;
                _message = message;
            }
        }
    }
}
