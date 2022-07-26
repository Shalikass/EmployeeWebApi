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
        public Mock<IEmployeeRepository> employeeRepositoryMock;
        public StatisticsService statisticsService;
        public Role role;

        public StatisticsServiceTests()
        {
            employeeRepositoryMock = new Mock<IEmployeeRepository>();
            statisticsService = new StatisticsService(employeeRepositoryMock.Object);
            role = new Role("A", default);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 800, 0)]
        [InlineData(1, 1111.11, 1111.11)]
        [InlineData(2, 2222.22, 1111.11)]
        [InlineData(7, 7000.70, 1000.10)]
        public async Task GetRoleStatistics_Statistics_ResultsValid(int employeeCount, decimal salarySum, decimal averageSalary)
        {
            role.Id = Guid.NewGuid();
            employeeRepositoryMock.Setup(m => 
                m.GetRoleEmployeeCountAsync(It.IsAny<Guid>())).ReturnsAsync(employeeCount);
            employeeRepositoryMock.Setup(m => 
                m.GetRoleEmployeeCurrentSalarySumAsync(It.IsAny<Guid>())).ReturnsAsync(salarySum);

            var statistics = await statisticsService.GetRoleStatisticsAsync(role);

            Assert.True(employeeCount == statistics.EmployeeCount && averageSalary == statistics.AverageSalary);
        }
    }
}
