using EmployeeWebApi.Models;

namespace EmployeeWebApi.Parsers
{
    public static class DataPrepHelpers
    {
        public static void Trim(this EmployeeUpdateDto employee)
        {
            employee.FirstName = employee.FirstName.Trim();
            employee.LastName = employee.LastName.Trim();
            employee.HomeAddress = employee.HomeAddress.Trim();
            employee.BirthDate = employee.BirthDate.Date;
            employee.EmploymentDate = employee.EmploymentDate.Date;
        }
    }
}
