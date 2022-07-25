using EmployeeWebApi.Models;
using EmployeeWebApi.Services;
using FluentValidation;

namespace EmployeeWebApi.Validators
{
    public class EmployeeValidator : AbstractValidator<EmployeeUpdateDto>
    {
        public EmployeeValidator()
        {
            Transform(x => x.FirstName, x => x?.Trim())
                .NotEmpty()
                .MaximumLength(50);
            Transform(x => x.LastName, x => x?.Trim())
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x)
                .Must(x => x.FirstName?.Trim() != x.LastName?.Trim())
                .WithMessage("First and last name must be different.");

            RuleFor(x => x.BirthDate)
                .NotNull()
                .Must(x => x > DateTime.Now.AddYears(-70) 
                        && x < DateTime.Now.AddYears(-18))
                .WithMessage("Employee age must be between 18 and 70 years old.");

            RuleFor(x => x.EmploymentDate)
                .NotNull()
                .Must(x => x >= new DateTime(2000,01,01) 
                        && x < DateTime.Now)
                .WithMessage("Employment date must be after 2000-01-01 and before tomorrow.");

            RuleFor(x => x.CurrentSalary)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.RoleId)
                .NotEmpty();

            RuleFor(x => x.HomeAddress)
                .NotEmpty();
        }
    }
}
