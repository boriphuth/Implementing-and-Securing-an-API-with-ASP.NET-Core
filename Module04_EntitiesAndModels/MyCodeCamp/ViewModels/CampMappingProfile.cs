using AutoMapper;
using MyCodeCamp.Data.Entities;

namespace MyCodeCamp.ViewModels
{
    public class CampMappingProfile : Profile
    {
        public CampMappingProfile() {
            CreateMap<Camp, CampViewModel>();
        }
    }
}
