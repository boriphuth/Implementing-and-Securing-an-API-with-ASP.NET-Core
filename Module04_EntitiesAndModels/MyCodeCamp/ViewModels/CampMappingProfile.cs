using AutoMapper;
using MyCodeCamp.Data.Entities;

namespace MyCodeCamp.ViewModels
{
    public class CampMappingProfile : Profile
    {
        public CampMappingProfile() {
            CreateMap<Camp, CampViewModel>()
                .ForMember(cvm => cvm.StartDate, opt => opt.MapFrom(cm => cm.EventDate))
                //.ForMember(cvm => cvm.EndDate, opt => opt.MapFrom(cm => cm.EventDate.AddDays(cm.Length - 1)));
                .ForMember(cvm => cvm.EndDate, opt => opt.ResolveUsing(cm => cm.EventDate.AddDays(cm.Length - 1)));
        }
    }
}
