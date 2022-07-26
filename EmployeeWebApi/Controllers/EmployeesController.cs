using AutoMapper;
using EmployeeWebApi.Models;
using EmployeeWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using EmployeeWebApi.Validators;
using EmployeeWebApi.Parsers;
using Newtonsoft.Json;

namespace EmployeeWebApi.Controllers
{
    [Route("api/employees")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeService _employeeService;
        const int maxPageSize = 50;

        public EmployeesController(ILogger<EmployeesController> logger, IMapper mapper, IEmployeeRepository employeeRepository, IEmployeeService employeeService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeGetDto>>> GetEmployees(
            string? searchQuery, Guid? bossId, DateTime? birthDateFrom, DateTime? birthDateTo, 
            int pageNumber = 1, int pageSize = 20)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var (employees, paginationMetadata) = await _employeeRepository.GetEmployeesAsync(
                searchQuery, bossId, birthDateFrom, birthDateTo, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<EmployeeGetDto>>(employees));
        }


        [HttpGet("{id}", Name = "GetEmployee")]
        public async Task<ActionResult<EmployeeGetDto>> GetEmployee(Guid id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<EmployeeGetDto>(employee));
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeUpdateDto>> CreateEmployee(
           EmployeeUpdateDto employeeCreate)
        {
            employeeCreate.Trim();
            var newEmployee = _mapper.Map<Entities.Employee>(employeeCreate);

            var result = await _employeeService.UpdateEmployeeAsync(newEmployee);
                
            switch(result.ResultCode)
            {
                case ResultCode.NotFound:
                    return NotFound(result.Message);
                case ResultCode.CannotProcess:
                    return BadRequest(result.Message);
                default:
                    break;
            }               

            var createdEmployee = _mapper.Map<EmployeeGetDto>(await _employeeRepository.GetEmployeeAsync(newEmployee.Id));
            return CreatedAtRoute("GetEmployee", new { id = createdEmployee.Id}, createdEmployee);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(Guid id)
        {
            var employeeEntity = await _employeeRepository.GetEmployeeAsync(id);
            if (employeeEntity == null)
            {
                return NotFound();
            }

            _employeeRepository.DeleteEmployee(employeeEntity);
            await _employeeRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee(Guid id, EmployeeUpdateDto employeeToUpdate)
        {
            employeeToUpdate.Trim();
            var employeeEntity = await _employeeRepository.GetEmployeeAsync(id);
            if (employeeEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(employeeToUpdate, employeeEntity);

            var result = await _employeeService.UpdateEmployeeAsync(employeeEntity);

            switch (result.ResultCode)
            {
                case ResultCode.NotFound:
                    return NotFound(result.Message);
                case ResultCode.CannotProcess:
                    return BadRequest(result.Message);
                default:
                    break;
            }

            return NoContent();
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdateEmployee(Guid id, JsonPatchDocument<EmployeeUpdateDto> patchDocument)
        {
            var employeeEntity = await _employeeRepository.GetEmployeeAsync(id);
            if (employeeEntity == null)
            {
                return NotFound();
            }

            var employeeToPatch = _mapper.Map<EmployeeUpdateDto>(employeeEntity);

            patchDocument.ApplyTo(employeeToPatch, ModelState);

            employeeToPatch.Trim();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(employeeToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(employeeToPatch, employeeEntity);

            var result = await _employeeService.UpdateEmployeeAsync(employeeEntity);

            switch (result.ResultCode)
            {
                case ResultCode.NotFound:
                    return NotFound(result.Message);
                case ResultCode.CannotProcess:
                    return BadRequest(result.Message);
                default:
                    break;
            }

            return NoContent();
        }
    }
}
