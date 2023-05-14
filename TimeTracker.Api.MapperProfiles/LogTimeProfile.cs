using AutoMapper;
using Services.Contracts.Models;
using TimeTracker.Api.Contracts;

namespace TimeTracker.Api.MapperProfiles
{
    public class LogTimeProfile : Profile
    {
        public LogTimeProfile()
        {
            CreateMap<CreateLogTimeApiModel, LogTimeRequestModel>();
        }
    }  
}
