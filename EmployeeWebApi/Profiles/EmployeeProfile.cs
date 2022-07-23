using AutoMapper;

namespace EmployeeWebApi.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Entities.Employee, Models.EmployeeDto>();
            CreateMap<Entities.Role, Models.RoleDto>();
        }
    }
}
