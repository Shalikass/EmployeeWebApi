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

        public async Task<IResult> UpdateEmployeeAsync(Employee employee)
        {
            var role = await _employeeRepository.GetRoleAsync(employee.RoleId);
            if (role == null)
            {
                return new Result(ResultCode.NotFound, $"Role with id \"{employee.RoleId}\" does not exist.");
            }

            if (role.Name == Constants.RoleNameCEO)
            {
                var currentCEO = await _employeeRepository.GetCEOAsync();
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
                var result = await CheckBossExistsAsync(employee);
                if (result.ResultCode != ResultCode.Success)
                    return result;
            }
            if (employee.Id == default)
            {
                _employeeRepository.AddEmployee(employee);
            }
            await _employeeRepository.SaveChangesAsync();
            return new Result(ResultCode.Success);
        }

        private async Task<IResult> CheckBossExistsAsync(Employee employee)
        {
            if (employee.BossId == null)
            {
                return new Result(ResultCode.CannotProcess, $"Boss id for this role cannot be null.");
            }
            if (!await _employeeRepository.EmployeeExistsAsync(employee.BossId))
            {
                return new Result(ResultCode.NotFound, $"Employee with id \"{employee.BossId}\" does not exist.");
            }
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
