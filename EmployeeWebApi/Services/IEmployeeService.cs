using EmployeeWebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWebApi.Services
{
    public interface IEmployeeService
    {
        Task<IResult> CreateEmployeeAsync(Employee employee);
        Task<IResult> UpdateEmployeeAsync(Employee employee);
              

    }
    public interface IResult
    {
        public ResultCode ResultCode { get; }
        public string? Message { get; }
    }

    public enum ResultCode
    {
        Success,
        NotFound,
        CannotProcess
    }
}
