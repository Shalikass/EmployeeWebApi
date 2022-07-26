using EmployeeWebApi.Entities;
using EmployeeWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeWebApi.Test
{
    public class EmployeeServiceTests
    {
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private EmployeeService _employeeService;
        private Employee _testEmployee;
        private Employee _ceoEmployee;
        private Role _testEmployeeRole;

        public EmployeeServiceTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _employeeRepositoryMock.Setup(m =>
                    m.AddEmployee(It.IsAny<Employee>())).Verifiable();
            _employeeRepositoryMock.Setup(m =>
                    m.SaveChangesAsync()).Verifiable();

            // Boss exists default = true
            _employeeRepositoryMock.Setup(m =>
                    m.EmployeeExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);


            // Normal employee defaults
            _testEmployee = new Employee("A", "A", default, default, "A", default, Guid.NewGuid(), Guid.NewGuid());
            _testEmployeeRole = new Role("A", default);
            _testEmployee.Id = Guid.Empty;

            // Normal ceo defaults
            _ceoEmployee = new Employee("A", "A", default, default, "A", default, null, Guid.NewGuid());
            _ceoEmployee.Id = Guid.NewGuid();
            _ceoEmployee.Role = new Role(Constants.RoleNameCEO, default);

            _employeeRepositoryMock.Setup(m =>
                    m.GetRoleAsync(It.IsAny<Guid>())).ReturnsAsync(_testEmployeeRole);

            _employeeService = new EmployeeService(_employeeRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateEmployee_AddEmployeeMethod_MustBeInvoked_Async()
        {
            var result = await _employeeService.UpdateEmployeeAsync(_testEmployee);

            _employeeRepositoryMock.Verify(m => m.AddEmployee(It.IsAny<Employee>()));
        }

        [Fact]
        public async Task CreateEmployee_SaveChangesAsync_MustBeInvoked_Async()
        {
            var result = await _employeeService.UpdateEmployeeAsync(_testEmployee);

            _employeeRepositoryMock.Verify(m => m.SaveChangesAsync());
        }

        [Fact]
        public async Task UpdateEmployee_SaveChangesAsync_MustBeInvoked_Async()
        {
            _testEmployee.Id = Guid.NewGuid();
            var result = await _employeeService.UpdateEmployeeAsync(_testEmployee);

            _employeeRepositoryMock.Verify(m => m.SaveChangesAsync());
        }

        [Theory]
        [InlineData(true, ResultCode.CannotProcess)]
        [InlineData(false, ResultCode.Success)]
        public async Task CreateEmployee_BossId_IsRequired_Async(bool isNull, ResultCode expected)
        {            
            _testEmployee.BossId = isNull ? null : Guid.NewGuid();
            _testEmployeeRole.Name = "A" + Constants.RoleNameCEO;

            var result = await _employeeService.UpdateEmployeeAsync(_testEmployee);
            Assert.Equal(expected, result.ResultCode);
        }

        [Theory]
        [InlineData(true, ResultCode.CannotProcess)]
        [InlineData(false, ResultCode.Success)]
        public async Task UpdateEmployee_BossId_IsRequired_Async(bool isNull, ResultCode expected)
        {
            _testEmployee.Id = Guid.NewGuid();
            _testEmployee.BossId = isNull ? null : Guid.NewGuid();
            _testEmployeeRole.Name = "A" + Constants.RoleNameCEO;

            var result = await _employeeService.UpdateEmployeeAsync(_testEmployee);
            Assert.Equal(expected, result.ResultCode);
        }

        [Theory]
        [InlineData(false, ResultCode.NotFound)]
        [InlineData(true, ResultCode.Success)]
        public async Task CreateEmployee_BossId_MustExist_Async(bool exists, ResultCode expected)
        {
            _testEmployee.BossId = Guid.NewGuid();
            _testEmployeeRole.Name = "A" + Constants.RoleNameCEO;

            _employeeRepositoryMock.Setup(m =>
                    m.EmployeeExistsAsync(It.IsAny<Guid>())).ReturnsAsync(exists);

            var result = await _employeeService.UpdateEmployeeAsync(_testEmployee);
            Assert.Equal(expected, result.ResultCode);
        }

        [Theory]
        [InlineData(false, ResultCode.NotFound)]
        [InlineData(true, ResultCode.Success)]
        public async Task UpdateEmployee_BossId_MustExist_Async(bool exists, ResultCode expected)
        {
            _testEmployee.Id = Guid.NewGuid();
            _testEmployee.BossId = Guid.NewGuid();
            _testEmployeeRole.Name = "A" + Constants.RoleNameCEO;

            _employeeRepositoryMock.Setup(m =>
                    m.EmployeeExistsAsync(It.IsAny<Guid>())).ReturnsAsync(exists);

            var result = await _employeeService.UpdateEmployeeAsync(_testEmployee);
            Assert.Equal(expected, result.ResultCode);
        }

        [Theory]
        [InlineData(false, ResultCode.CannotProcess)]
        [InlineData(true, ResultCode.Success)]
        public async Task CreateCeoEmployee_BossId_MustBeNull_Async(bool isNull, ResultCode expected)
        {
            _testEmployeeRole.Name = Constants.RoleNameCEO;
            _testEmployee.BossId = isNull ? null : Guid.NewGuid();

            _employeeRepositoryMock.Setup(m =>
                    m.GetCEOAsync()).ReturnsAsync((Employee?)null);

            var result = await _employeeService.UpdateEmployeeAsync(_testEmployee);
            Assert.Equal(expected, result.ResultCode);
        }

        [Theory]
        [InlineData(false, ResultCode.CannotProcess)]
        [InlineData(true, ResultCode.Success)]
        public async Task UpdateCeoEmployee_BossId_MustBeNull_Async(bool isNull, ResultCode expected)
        {
            _testEmployee.Id = _ceoEmployee.Id;
            _testEmployeeRole.Name = Constants.RoleNameCEO;
            _testEmployee.BossId = isNull ? null : Guid.NewGuid();

            _employeeRepositoryMock.Setup(m =>
                    m.GetCEOAsync()).ReturnsAsync(_ceoEmployee);

            var result = await _employeeService.UpdateEmployeeAsync(_testEmployee);
            Assert.Equal(expected, result.ResultCode);
        }

        [Theory]
        [InlineData(true, ResultCode.CannotProcess)]
        [InlineData(false, ResultCode.Success)]
        public async Task CreateEmployee_OnlyOneEmployeeCanHaveRoleCeo_Async(bool ceoExists, ResultCode expected)
        {
            _testEmployee.BossId = null;
            _testEmployeeRole.Name = Constants.RoleNameCEO;

            var ceo = ceoExists ? _ceoEmployee : null;

            _employeeRepositoryMock.Setup(m =>
                    m.GetCEOAsync()).ReturnsAsync(ceo);

            var result = await _employeeService.UpdateEmployeeAsync(_testEmployee);
            Assert.Equal(expected, result.ResultCode);
        }

        [Theory]
        [InlineData(true, ResultCode.CannotProcess)]
        [InlineData(false, ResultCode.Success)]
        public async Task UpdateEmployee_OnlyOneEmployeeCanHaveRoleCeo_Async(bool ceoExists, ResultCode expected)
        {
            _testEmployee.Id = Guid.NewGuid();
            _testEmployee.BossId = null;
            _testEmployeeRole.Name = Constants.RoleNameCEO;

            var ceo = ceoExists ? _ceoEmployee : null;

            _employeeRepositoryMock.Setup(m =>
                    m.GetCEOAsync()).ReturnsAsync(ceo);

            var result = await _employeeService.UpdateEmployeeAsync(_testEmployee);
            Assert.Equal(expected, result.ResultCode);
        }

        

    }
}