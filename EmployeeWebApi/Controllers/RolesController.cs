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
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IStatisticsService _statisticsService;
        public RolesController(ILogger<RolesController> logger, IMapper mapper, IEmployeeRepository employeeRepository, IStatisticsService statisticsService)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _employeeRepository = employeeRepository ??
                throw new ArgumentNullException(nameof(employeeRepository));
            _statisticsService = statisticsService ??
                throw new ArgumentNullException(nameof(statisticsService));
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

                var statistics = await _statisticsService.GetRoleStatisticsAsync(role);

                var result = _mapper.Map<RoleWithStatisticsDto>(role);
                result.Statistics = _mapper.Map<RoleStatisticsDto>(statistics);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Exception when getting role with id: {id} statistics.");
                return StatusCode(500, "A problem occurred while handling your request");
            }
        }
    }
}
