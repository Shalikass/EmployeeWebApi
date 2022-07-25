using AutoMapper;
using EmployeeWebApi.Models;
using EmployeeWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWebApi.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public RolesController(ILogger<RolesController> logger, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _employeeRepository = employeeRepository ??
                throw new ArgumentNullException(nameof(employeeRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            try
            {
                var roles = await _employeeRepository.GetRolesAsync();

                return Ok(_mapper.Map<IEnumerable<RoleDto>>(roles));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Exception when getting roles.");
                return StatusCode(500, "A problem occurred while handling your request");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(Guid id)
        {
            try
            {
                var role = await _employeeRepository.GetRoleAsync(id);
                if (role == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<RoleDto>(role));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Exception when getting role with id: {id} .");
                return StatusCode(500, "A problem occurred while handling your request");
            }
        }

        [HttpGet("{id}/statistics")]
        public async Task<ActionResult<RoleStatisticsDto>> GetRoleStatistics(Guid id)
        {
            try
            {
                var role = await _employeeRepository.GetRoleAsync(id);
                if (role == null)
                {
                    return NotFound();
                }
                var employeeCount = await _employeeRepository.GetRoleEmployeeCountAsync(id);
                var salarySum = await _employeeRepository.GetRoleEmployeeCurrentSalarySum(id);
                var roleStatistics = _mapper.Map<RoleStatisticsGetDto>(role);
                roleStatistics.Statistics.EmployeeCount = employeeCount;
                roleStatistics.Statistics.AverageSalary = (employeeCount == 0) ? 0 : (salarySum / employeeCount);

                return Ok(roleStatistics);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Exception when getting role with id: {id} statistics.");
                return StatusCode(500, "A problem occurred while handling your request");
            }
        }
    }
}
