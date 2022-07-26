﻿using AutoMapper;

namespace EmployeeWebApi.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Entities.Role, Models.RoleDto>();
            CreateMap<Entities.Role, Models.RoleWithStatisticsDto>();
            CreateMap<Entities.RoleStatistics, Models.RoleStatisticsDto>();
        }
    }
}
