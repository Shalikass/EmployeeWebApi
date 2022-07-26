using AutoMapper;
using EmployeeWebApi.Models;
using EmployeeWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EmployeeWebApi.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class RolesController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IStatisticsService _statisticsService;
        const int _maxPageSize = 50;

        public RolesController(ILogger<RolesController> logger, IMapper mapper, IEmployeeRepository employeeRepository, IStatisticsService statisticsService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles(
            string? searchQuery, int pageNumber = 1, int pageSize = 20)
        {
            if (pageSize > _maxPageSize)
            {
                pageSize = _maxPageSize;
            }
            var (roles, paginationMetadata) = await _employeeRepository.GetRolesAsync(searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<RoleDto>>(roles));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(Guid id)
        {
            var role = await _employeeRepository.GetRoleAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<RoleDto>(role));
        }

        [HttpGet("{id}/statistics")]
        public async Task<ActionResult<RoleStatisticsDto>> GetRoleStatistics(Guid id)
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
    }
}
