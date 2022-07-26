using EmployeeWebApi.Entities;
using EmployeeWebApi.Services;
using EmployeeWebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeWebApi.Test
{
    public class CustomValidationsTests
    {
        Guid ceoRoleId;
        Employee employeeTest;
        Employee employeeCEO;
        Role roleTest;

        public CustomValidationsTests()
        {
            ceoRoleId = Guid.NewGuid();
            employeeTest = new Employee("A", "A", default, default, "A", default, default);
            employeeCEO = new Employee("A", "A", default, default, "A", default, default);
            employeeCEO.Id = Guid.NewGuid();
            employeeCEO.RoleId = ceoRoleId;
            employeeCEO.Role = new Role(Constants.RoleNameCEO, default);
            roleTest = new Role("A", default);
        }

        [Fact]
        public async Task CheckCreationConstraints_EmployeeBossId_IsRequired_Async()
        {
            employeeTest.RoleId = Guid.NewGuid();
            employeeTest.BossId = null;
            roleTest.Id = employeeTest.RoleId;
            roleTest.Name = "A" + Constants.RoleNameCEO;

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            employeeRepositoryMock.Setup(m => 
                    m.GetRoleAsync(It.IsAny<Guid>())).ReturnsAsync(roleTest);

            var result = await CustomValidations.CheckCreateConstraints(employeeTest, employeeRepositoryMock.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CheckCreationConstraints_EmployeeBossId_MustExist_Async()
        {
            employeeTest.RoleId = Guid.NewGuid();
            employeeTest.BossId = Guid.NewGuid();
            roleTest.Id = employeeTest.RoleId;
            roleTest.Name = "A" + Constants.RoleNameCEO;

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            employeeRepositoryMock.Setup(m =>
                    m.GetRoleAsync(It.IsAny<Guid>())).ReturnsAsync(roleTest);
            employeeRepositoryMock.Setup(m =>
                    m.EmployeeExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var result = await CustomValidations.CheckCreateConstraints(employeeTest, employeeRepositoryMock.Object);
            Assert.Null(result);

            employeeRepositoryMock.Setup(m =>
                    m.EmployeeExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            result = await CustomValidations.CheckCreateConstraints(employeeTest, employeeRepositoryMock.Object);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CheckCreationConstraints_EmployeeRole_OnlyOneEmployeeCanHaveRoleCeo_Async()
        {
            employeeTest.RoleId = ceoRoleId;
            employeeTest.BossId = null;
            roleTest.Id = employeeTest.RoleId;
            roleTest.Name = Constants.RoleNameCEO;

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            employeeRepositoryMock.Setup(m =>
                    m.GetRoleAsync(It.IsAny<Guid>())).ReturnsAsync(roleTest);
            employeeRepositoryMock.Setup(m =>
                    m.GetRoleEmployeeCountAsync(It.IsAny<Guid>())).ReturnsAsync(1);

            var result = await CustomValidations.CheckCreateConstraints(employeeTest, employeeRepositoryMock.Object);
            Assert.IsType<BadRequestObjectResult>(result);

            employeeRepositoryMock.Setup(m =>
                    m.GetRoleEmployeeCountAsync(It.IsAny<Guid>())).ReturnsAsync(0);
            result = await CustomValidations.CheckCreateConstraints(employeeTest, employeeRepositoryMock.Object);
            Assert.Null(result);
        }

        [Fact]
        public async Task CheckCreationConstraints_CeoEmployeeBossId_MustBeNull_Async()
        {
            employeeTest.RoleId = ceoRoleId;            
            roleTest.Id = employeeTest.RoleId;
            roleTest.Name = Constants.RoleNameCEO;

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            employeeRepositoryMock.Setup(m =>
                    m.GetRoleAsync(It.IsAny<Guid>())).ReturnsAsync(roleTest);
            employeeRepositoryMock.Setup(m =>
                    m.GetRoleEmployeeCountAsync(It.IsAny<Guid>())).ReturnsAsync(0);

            employeeTest.BossId = Guid.NewGuid();
            var result = await CustomValidations.CheckCreateConstraints(employeeTest, employeeRepositoryMock.Object);
            Assert.IsType<BadRequestObjectResult>(result);

            employeeTest.BossId = null;
            result = await CustomValidations.CheckCreateConstraints(employeeTest, employeeRepositoryMock.Object);
            Assert.Null(result);
        }


        [Fact]
        public async Task CheckUpdateConstraints_EmployeeBossId_IsRequired_Async()
        {
            employeeTest.RoleId = Guid.NewGuid();
            employeeTest.BossId = null;
            roleTest.Id = employeeTest.RoleId;
            roleTest.Name = "A" + Constants.RoleNameCEO;

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            employeeRepositoryMock.Setup(m =>
                    m.GetRoleAsync(It.IsAny<Guid>())).ReturnsAsync(roleTest);

            var result = await CustomValidations.CheckUpdateConstraints(employeeTest, employeeRepositoryMock.Object);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CheckUpdateConstraints_EmployeeBossId_MustExist_Async()
        {
            employeeTest.RoleId = Guid.NewGuid();
            employeeTest.BossId = Guid.NewGuid();
            roleTest.Id = employeeTest.RoleId;
            roleTest.Name = "A" + Constants.RoleNameCEO;

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            employeeRepositoryMock.Setup(m =>
                    m.GetRoleAsync(It.IsAny<Guid>())).ReturnsAsync(roleTest);
            employeeRepositoryMock.Setup(m =>
                    m.EmployeeExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var result = await CustomValidations.CheckUpdateConstraints(employeeTest, employeeRepositoryMock.Object);
            Assert.Null(result);

            employeeRepositoryMock.Setup(m =>
                    m.EmployeeExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);
            result = await CustomValidations.CheckUpdateConstraints(employeeTest, employeeRepositoryMock.Object);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CheckUpdateConstraints_EmployeeRole_OnlyOneEmployeeCanHaveRoleCeo_Async()
        {
            employeeTest.RoleId = ceoRoleId;
            employeeTest.BossId = null;
            roleTest.Id = employeeTest.RoleId;
            roleTest.Name = Constants.RoleNameCEO;

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            employeeRepositoryMock.Setup(m =>
                    m.GetRoleAsync(It.IsAny<Guid>())).ReturnsAsync(roleTest);
            employeeRepositoryMock.Setup(m =>
                    m.GetCEOAsync()).ReturnsAsync(employeeCEO);


            employeeTest.Id = Guid.NewGuid();
            var result = await CustomValidations.CheckUpdateConstraints(employeeTest, employeeRepositoryMock.Object);
            Assert.IsType<BadRequestObjectResult>(result);

            employeeTest.Id = employeeCEO.Id;
            result = await CustomValidations.CheckUpdateConstraints(employeeTest, employeeRepositoryMock.Object);
            Assert.Null(result);

            employeeTest.Id = Guid.NewGuid();
            employeeRepositoryMock.Setup(m =>
                    m.GetCEOAsync()).ReturnsAsync((Employee?)null);
            result = await CustomValidations.CheckUpdateConstraints(employeeTest, employeeRepositoryMock.Object);
            Assert.Null(result);
        }

        [Fact]
        public async Task CheckUpdateConstraints_CeoEmployeeBossId_MustBeNull_Async()
        {
            employeeTest.RoleId = ceoRoleId;
            roleTest.Id = employeeTest.RoleId;
            roleTest.Name = Constants.RoleNameCEO;
            employeeTest.Id = employeeCEO.Id;

            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            employeeRepositoryMock.Setup(m =>
                    m.GetRoleAsync(It.IsAny<Guid>())).ReturnsAsync(roleTest);
            employeeRepositoryMock.Setup(m =>
                    m.GetCEOAsync()).ReturnsAsync(employeeCEO);


            employeeTest.BossId = Guid.NewGuid();
            var result = await CustomValidations.CheckUpdateConstraints(employeeTest, employeeRepositoryMock.Object);
            Assert.IsType<BadRequestObjectResult>(result);

            employeeTest.BossId = null;
            result = await CustomValidations.CheckUpdateConstraints(employeeTest, employeeRepositoryMock.Object);
            Assert.Null(result);
        }

    }
}