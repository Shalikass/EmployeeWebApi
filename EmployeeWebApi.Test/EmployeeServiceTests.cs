using EmployeeWebApi.Entities;
using EmployeeWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeWebApi.Test
{
    public class EmployeeServiceTests
    {
        public Mock<IEmployeeRepository> employeeRepositoryMock;
        public EmployeeService employeeService;
        Guid ceoRoleId;
        Employee testEmploee;
        Employee ceoEmployee;
        Role testEmployeeRole;

        public EmployeeServiceTests()
        {
            employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.Setup(m =>
                    m.SaveChangesAsync()).Verifiable();

            employeeService = new EmployeeService(employeeRepositoryMock.Object);

            ceoRoleId = Guid.NewGuid();
            testEmploee = new Employee("A", "A", default, default, "A", default, default);
            ceoEmployee = new Employee("A", "A", default, default, "A", default, default);
            ceoEmployee.Id = Guid.NewGuid();
            ceoEmployee.RoleId = ceoRoleId;
            ceoEmployee.Role = new Role(Constants.RoleNameCEO, default);
            testEmployeeRole = new Role("A", default);

            employeeRepositoryMock.Setup(m =>
                    m.GetRoleAsync(It.IsAny<Guid>())).ReturnsAsync(testEmployeeRole);
        }

        [Fact]
        public async Task CreateEmployee_EmployeeBossId_IsRequired_Async()
        {
            testEmploee.RoleId = Guid.NewGuid();
            testEmploee.BossId = null;
            testEmployeeRole.Id = testEmploee.RoleId;
            testEmployeeRole.Name = "A" + Constants.RoleNameCEO;                      

            var result = await employeeService.CreateEmployeeAsync(testEmploee);
            Assert.Equal(ResultCode.CannotProcess, result.ResultCode);
        }

        [Fact]
        public async Task CreateEmployee_EmployeeBossId_MustExist_Async()
        {
            testEmploee.RoleId = Guid.NewGuid();
            testEmploee.BossId = Guid.NewGuid();
            testEmployeeRole.Id = testEmploee.RoleId;
            testEmployeeRole.Name = "A" + Constants.RoleNameCEO;

            employeeRepositoryMock.Setup(m =>
                    m.EmployeeExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var result = await employeeService.CreateEmployeeAsync(testEmploee);
            employeeRepositoryMock.Verify(m => m.SaveChangesAsync());
            Assert.Equal(ResultCode.Success, result.ResultCode);

            employeeRepositoryMock.Setup(m =>
                    m.EmployeeExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            result = await employeeService.CreateEmployeeAsync(testEmploee);
            Assert.Equal(ResultCode.NotFound, result.ResultCode);
        }

        [Fact]
        public async Task CreateEmployee_EmployeeRole_OnlyOneEmployeeCanHaveRoleCeo_Async()
        {
            testEmploee.RoleId = ceoRoleId;
            testEmploee.BossId = null;
            testEmployeeRole.Id = testEmploee.RoleId;
            testEmployeeRole.Name = Constants.RoleNameCEO;

            employeeRepositoryMock.Setup(m =>
                    m.GetRoleEmployeeCountAsync(It.IsAny<Guid>())).ReturnsAsync(1);

            var result = await employeeService.CreateEmployeeAsync(testEmploee);
            Assert.Equal(ResultCode.CannotProcess, result.ResultCode);

            employeeRepositoryMock.Setup(m =>
                    m.GetRoleEmployeeCountAsync(It.IsAny<Guid>())).ReturnsAsync(0);;

            result = await employeeService.CreateEmployeeAsync(testEmploee);
            employeeRepositoryMock.Verify(m => m.SaveChangesAsync());
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async Task CreateEmployee_CeoEmployeeBossId_MustBeNull_Async()
        {
            testEmploee.RoleId = ceoRoleId;
            testEmployeeRole.Id = testEmploee.RoleId;
            testEmployeeRole.Name = Constants.RoleNameCEO;

            employeeRepositoryMock.Setup(m =>
                    m.GetRoleEmployeeCountAsync(It.IsAny<Guid>())).ReturnsAsync(0);

            testEmploee.BossId = Guid.NewGuid();
            var result = await employeeService.CreateEmployeeAsync(testEmploee);
            Assert.Equal(ResultCode.CannotProcess, result.ResultCode);
            employeeRepositoryMock.Setup(m =>
                    m.SaveChangesAsync()).Verifiable();

            testEmploee.BossId = null;
            result = await employeeService.CreateEmployeeAsync(testEmploee);
            employeeRepositoryMock.VerifyAll();
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }


        [Fact]
        public async Task UpdateEmployee_EmployeeBossId_IsRequired_Async()
        {
            testEmploee.RoleId = Guid.NewGuid();
            testEmploee.BossId = null;
            testEmployeeRole.Id = testEmploee.RoleId;
            testEmployeeRole.Name = "A" + Constants.RoleNameCEO;

            var result = await employeeService.UpdateEmployeeAsync(testEmploee);
            Assert.Equal(ResultCode.CannotProcess, result.ResultCode);
        }

        [Fact]
        public async Task UpdateEmployee_EmployeeBossId_MustExist_Async()
        {
            testEmploee.RoleId = Guid.NewGuid();
            testEmploee.BossId = Guid.NewGuid();
            testEmployeeRole.Id = testEmploee.RoleId;
            testEmployeeRole.Name = "A" + Constants.RoleNameCEO;

            employeeRepositoryMock.Setup(m =>
                    m.EmployeeExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            var result = await employeeService.UpdateEmployeeAsync(testEmploee);
            Assert.Equal(ResultCode.NotFound, result.ResultCode);

            employeeRepositoryMock.Setup(m =>
                    m.EmployeeExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            result = await employeeService.UpdateEmployeeAsync(testEmploee);
            employeeRepositoryMock.Verify(m => m.SaveChangesAsync());
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async Task UpdateEmployee_EmployeeRole_OnlyOneEmployeeCanHaveRoleCeo_Async()
        {
            testEmploee.RoleId = ceoRoleId;
            testEmploee.BossId = null;
            testEmployeeRole.Id = testEmploee.RoleId;
            testEmployeeRole.Name = Constants.RoleNameCEO;

            employeeRepositoryMock.Setup(m =>
                    m.GetCEOAsync()).ReturnsAsync(ceoEmployee);


            testEmploee.Id = Guid.NewGuid();
            var result = await employeeService.UpdateEmployeeAsync(testEmploee);
            Assert.Equal(ResultCode.CannotProcess, result.ResultCode);

            testEmploee.Id = ceoEmployee.Id;
            result = await employeeService.UpdateEmployeeAsync(testEmploee);
            Assert.Equal(ResultCode.Success, result.ResultCode);

            testEmploee.Id = Guid.NewGuid();
            employeeRepositoryMock.Setup(m =>
                    m.GetCEOAsync()).ReturnsAsync((Employee?)null);
            result = await employeeService.UpdateEmployeeAsync(testEmploee);
            employeeRepositoryMock.Verify(m => m.SaveChangesAsync());
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

        [Fact]
        public async Task UpdateEmployee_CeoEmployeeBossId_MustBeNull_Async()
        {
            testEmploee.RoleId = ceoRoleId;
            testEmployeeRole.Id = testEmploee.RoleId;
            testEmployeeRole.Name = Constants.RoleNameCEO;
            testEmploee.Id = ceoEmployee.Id;

            employeeRepositoryMock.Setup(m =>
                    m.GetCEOAsync()).ReturnsAsync(ceoEmployee);


            testEmploee.BossId = Guid.NewGuid();
            var result = await employeeService.UpdateEmployeeAsync(testEmploee);
            Assert.Equal(ResultCode.CannotProcess, result.ResultCode);

            testEmploee.BossId = null;
            result = await employeeService.UpdateEmployeeAsync(testEmploee);
            employeeRepositoryMock.Verify(m => m.SaveChangesAsync());
            Assert.Equal(ResultCode.Success, result.ResultCode);
        }

    }
}