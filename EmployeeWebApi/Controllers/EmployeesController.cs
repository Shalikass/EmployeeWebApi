using AutoMapper;
using EmployeeWebApi.Models;
using EmployeeWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWebApi.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public EmployeesController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository ??
                throw new ArgumentNullException(nameof(employeeRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees(
            string? searchQuery, Guid? bossId, DateTime? birthDateFrom, DateTime? birthDateTo)
        {
            var employees = await _employeeRepository.GetEmployeesAsync(searchQuery, bossId, birthDateFrom, birthDateTo);

            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }


        [HttpGet("{id}", Name = "GetEmployee")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(Guid id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<EmployeeDto>(employee));
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeCreateDto>> CreateEmployee(
           EmployeeCreateDto employeeCreate)
        {
            var newEmployee = _mapper.Map<Entities.Employee>(employeeCreate);

            _employeeRepository.AddEmployee(newEmployee);

            await _employeeRepository.SaveChangesAsync();

            var createdEmployee = _mapper.Map<EmployeeDto>(await _employeeRepository.GetEmployeeAsync(newEmployee.Id));

            return CreatedAtRoute("GetEmployee",
                new
                {
                    id = createdEmployee.Id
                }
                ,createdEmployee);
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
            var employeeEntity = await _employeeRepository.GetEmployeeAsync(id);
            if (employeeEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(employeeToUpdate, employeeEntity);

            await _employeeRepository.SaveChangesAsync();

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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(employeeToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(employeeToPatch, employeeEntity);
            await _employeeRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
