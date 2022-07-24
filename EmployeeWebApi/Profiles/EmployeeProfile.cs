using AutoMapper;

namespace EmployeeWebApi.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Entities.Employee, Models.EmployeeGetDto>();
            CreateMap<Entities.Employee, Models.EmployeeUpdateDto>();
            CreateMap<Models.EmployeeUpdateDto, Entities.Employee>();
        }
    }
}
