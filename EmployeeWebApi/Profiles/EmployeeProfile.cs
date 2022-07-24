using AutoMapper;

namespace EmployeeWebApi.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Entities.Employee, Models.EmployeeDto>();
            CreateMap<Models.EmployeeCreateDto, Entities.Employee>();
            CreateMap<Entities.Employee, Models.EmployeeUpdateDto>();
            CreateMap<Models.EmployeeUpdateDto, Entities.Employee>();
        }
    }
}
