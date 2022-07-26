using EmployeeWebApi.Models;
using EmployeeWebApi.Validators;
using EmployeeWebApi.Test.Fixtures;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeWebApi.Test
{

    //var employeeDto = new EmployeeUpdateDto {FirstName = "First", LastName = "Last", BirthDate = new Date };
    public class UpdateValidationTests : IClassFixture<EmployeeValidatorFixture>
    {
        private readonly EmployeeValidatorFixture _employeeValidatorFixture;
        
        static string str51 = new string('A', 51);
        static string str50 = new string('A', 50);

        public UpdateValidationTests(EmployeeValidatorFixture employeeValidatorFixture)
        {
            _employeeValidatorFixture = employeeValidatorFixture;
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void EmployeeUpdateValidator_Properties_IsRequiredAndNotEmpty(string input)
        {
            // FirstName
            var employee = new EmployeeUpdateDto { FirstName = input };
            var result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);

            // LastName
            employee = new EmployeeUpdateDto { LastName = input };
            result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
            
            // HomeAddress
            employee = new EmployeeUpdateDto { HomeAddress = input };
            result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldHaveValidationErrorFor(x => x.HomeAddress);
        }

        [Fact]
        public void EmployeeUpdateValidator_FirstNameAndLastName_MustBeShorterThan50()
        {
            // FirstName
            var employee = new EmployeeUpdateDto { FirstName = str51 };
            var result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);

            employee = new EmployeeUpdateDto { FirstName = str50 };
            result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);

            // LastName
            employee = new EmployeeUpdateDto { LastName = str51 };
            result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldHaveValidationErrorFor(x => x.LastName);

            employee = new EmployeeUpdateDto { LastName = str50 };
            result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        }

        [Theory]
        [InlineData("AA", "AA")]
        [InlineData("  AA  ", "AA")]
        [InlineData("AA", "  AA  ")]
        public void EmployeeUpdateValidator_FirstNameAndLastName_MustBeDifferent(string firstName, string lastName)
        {
            var employee = new EmployeeUpdateDto { FirstName = firstName, LastName = lastName };
            var result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldHaveValidationErrorFor(x => x);
        }

        [Fact]
        public void EmployeeUpdateValidator_BirthDate_AgeMustBeBetween18And70()
        {
            var employee = new EmployeeUpdateDto { BirthDate = DateTime.Today.AddYears(-70)};
            var result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldHaveValidationErrorFor(x => x.BirthDate);

            employee = new EmployeeUpdateDto { BirthDate = DateTime.Today.AddYears(-18).AddDays(1) };
            result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldHaveValidationErrorFor(x => x.BirthDate);

            employee = new EmployeeUpdateDto { BirthDate = DateTime.Today.AddYears(-70).AddDays(1) };
            result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldNotHaveValidationErrorFor(x => x.BirthDate);

            employee = new EmployeeUpdateDto { BirthDate = DateTime.Today.AddYears(-18).AddMinutes(1)};
            result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldNotHaveValidationErrorFor(x => x.BirthDate);
        }

        [Fact]
        public void EmployeeUpdateValidator_EmploymentDate_MustBeAfter2000_01_01AndBeforeToday()
        {
            var employee = new EmployeeUpdateDto { EmploymentDate = new DateTime(1999, 12, 31) };
            var result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldHaveValidationErrorFor(x => x.EmploymentDate);

            employee = new EmployeeUpdateDto { EmploymentDate = DateTime.Today.AddDays(1) };
            result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldHaveValidationErrorFor(x => x.EmploymentDate);

            employee = new EmployeeUpdateDto { EmploymentDate = new DateTime(2000, 01, 01) };
            result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldNotHaveValidationErrorFor(x => x.EmploymentDate);

            employee = new EmployeeUpdateDto { EmploymentDate = DateTime.Today};
            result = _employeeValidatorFixture.employeeValidator.TestValidate(employee);
            result.ShouldNotHaveValidationErrorFor(x => x.EmploymentDate);
        }

    }
}
