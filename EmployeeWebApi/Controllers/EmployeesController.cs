using AutoMapper;
using EmployeeWebApi.Models;
using EmployeeWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using EmployeeWebApi.Validators;
using EmployeeWebApi.Parsers;

namespace EmployeeWebApi.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public EmployeesController(ILogger<EmployeesController> logger, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _employeeRepository = employeeRepository ??
                throw new ArgumentNullException(nameof(employeeRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeGetDto>>> GetEmployees(
            string? searchQuery, Guid? bossId, DateTime? birthDateFrom, DateTime? birthDateTo)
        {
            try
            {
                var employees = await _employeeRepository.GetEmployeesAsync(searchQuery, bossId, birthDateFrom, birthDateTo);
                return Ok(_mapper.Map<IEnumerable<EmployeeGetDto>>(employees));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Exception when getting employees.");
                return StatusCode(500, "A problem occurred while handling your request");
            }
        }


        [HttpGet("{id}", Name = "GetEmployee")]
        public async Task<ActionResult<EmployeeGetDto>> GetEmployee(Guid id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<EmployeeGetDto>(employee));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Exception when getting employee with id: {id} .");
                return StatusCode(500, "A problem occurred while handling your request");
            }
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeUpdateDto>> CreateEmployee(
           EmployeeUpdateDto employeeCreate)
        {
            try
            {
                EmployeeParsers.TransformEmployeeUpdateDto(employeeCreate);
                var newEmployee = _mapper.Map<Entities.Employee>(employeeCreate);

                var validationResult = await CustomValidations.CheckCreateConstraints(newEmployee, _employeeRepository);
                if (validationResult != null)
                    return validationResult;

                _employeeRepository.AddEmployee(newEmployee);
                await _employeeRepository.SaveChangesAsync();

                var createdEmployee = _mapper.Map<EmployeeGetDto>(await _employeeRepository.GetEmployeeAsync(newEmployee.Id));
                return CreatedAtRoute("GetEmployee",
                    new
                    {
                        id = createdEmployee.Id
                    }
                    , createdEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Exception when creating employee.");
                return StatusCode(500, "A problem occurred while handling your request");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Exception when deleting employee");
                return StatusCode(500, "A problem occurred while handling your request");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee(Guid id, EmployeeUpdateDto employeeToUpdate)
        {
            try
            {
                EmployeeParsers.TransformEmployeeUpdateDto(employeeToUpdate);
                var employeeEntity = await _employeeRepository.GetEmployeeAsync(id);
                if (employeeEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(employeeToUpdate, employeeEntity);

                var validationResult = await CustomValidations.CheckUpdateConstraints(employeeEntity, _employeeRepository);
                if (validationResult != null)
                    return validationResult;

                await _employeeRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Exception when updating employee with id: {id} .");
                return StatusCode(500, "A problem occurred while handling your request");
            }
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdateEmployee(Guid id, JsonPatchDocument<EmployeeUpdateDto> patchDocument)
        {
            try
            {
                var employeeEntity = await _employeeRepository.GetEmployeeAsync(id);
                if (employeeEntity == null)
                {
                    return NotFound();
                }

                var employeeToPatch = _mapper.Map<EmployeeUpdateDto>(employeeEntity);

                patchDocument.ApplyTo(employeeToPatch, ModelState);

                EmployeeParsers.TransformEmployeeUpdateDto(employeeToPatch);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (!TryValidateModel(employeeToPatch))
                {
                    return BadRequest(ModelState);
                }

                bool needValidation = (employeeToPatch.RoleId != employeeEntity.RoleId
                                      || employeeToPatch.BossId == null
                                      || employeeToPatch.BossId != employeeEntity.BossId);

                _mapper.Map(employeeToPatch, employeeEntity);

                if (needValidation)
                {
                    var validationResult = await CustomValidations.CheckUpdateConstraints(employeeEntity, _employeeRepository);
                    if (validationResult != null)
                        return validationResult;
                }

                await _employeeRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Exception when patching employee with id: {id} .");
                return StatusCode(500, "A problem occurred while handling your request");
            }
        }
    }
}
