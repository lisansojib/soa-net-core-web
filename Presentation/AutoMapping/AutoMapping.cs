using ApplicationCore.Entities;
using AutoMapper;
using Presentation.Models;

namespace Presentation.AutoMapping
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<RegisterBindingModel, User>();
        }
    }
}
