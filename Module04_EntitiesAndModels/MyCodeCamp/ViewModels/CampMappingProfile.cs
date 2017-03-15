using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Data.Entities;

namespace MyCodeCamp.ViewModels
{
    public class CampMappingProfile : Profile
    {
        public CampMappingProfile() {
            CreateMap<Camp, CampViewModel>()
                /*.ForMember(cvm => cvm.Url, 
                    opt => opt.ResolveUsing((camp, model, _, ctx) => {
                        var url = (IUrlHelper)ctx.Items["UrlHelper"];
                        return url.Link("GetCamp", new { id = camp.Id });
                    })                   
                )*/ /* => this method was cumbersome to use, now implemented by custom resolver */
                .ForMember(cvm => cvm.Url,
                    opt => opt.ResolveUsing<CampUrlResolver>())
                .ForMember(cvm => cvm.StartDate, 
                    opt => opt.MapFrom(cm => cm.EventDate))
                /*.ForMember(cvm => cvm.EndDate, 
                    opt => opt.MapFrom(cm => cm.EventDate.AddDays(cm.Length - 1)));*/
                .ForMember(cvm => cvm.EndDate, 
                    opt => opt.ResolveUsing(cm => cm.EventDate.AddDays(cm.Length - 1)));
        }
    }
}
