using EmployeeWebApi.Entities;
using EmployeeWebApi.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeWebApi.Test
{
    public class StatisticsServiceTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private StatisticsService _statisticsService;
        private Role _role;

        public StatisticsServiceTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _statisticsService = new StatisticsService(_employeeRepositoryMock.Object);
            _role = new Role("A", default);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 800, 0)]
        [InlineData(1, 1111.11, 1111.11)]
        [InlineData(2, 2222.22, 1111.11)]
        [InlineData(7, 7000.70, 1000.10)]
        public async Task GetRoleStatistics_Statistics_ResultsValid(int employeeCount, decimal salarySum, decimal averageSalary)
        {
            _role.Id = Guid.NewGuid();
            _employeeRepositoryMock.Setup(m => 
                m.GetRoleEmployeeCountAsync(It.IsAny<Guid>())).ReturnsAsync(employeeCount);
            _employeeRepositoryMock.Setup(m => 
                m.GetRoleEmployeeCurrentSalarySumAsync(It.IsAny<Guid>())).ReturnsAsync(salarySum);

            var statistics = await _statisticsService.GetRoleStatisticsAsync(_role);

            Assert.True(employeeCount == statistics.EmployeeCount && averageSalary == statistics.AverageSalary);
        }
    }
}
