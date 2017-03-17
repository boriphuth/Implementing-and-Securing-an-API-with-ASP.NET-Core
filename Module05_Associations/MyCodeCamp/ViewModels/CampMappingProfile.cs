using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Data.Entities;

namespace MyCodeCamp.ViewModels
{
    public class CampMappingProfile : Profile
    {
        public CampMappingProfile() {
            CreateMap<Speaker, SpeakerViewModel>()
                .ForMember(svm => svm.Url,
                    opt => opt.ResolveUsing<SpeakerUrlResolver>())
                .ReverseMap();

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
                    opt => opt.ResolveUsing(cm => cm.EventDate.AddDays(cm.Length - 1)))

                .ReverseMap()

                .ForMember(cm => cm.EventDate, 
                    opt => opt.MapFrom(cvm => cvm.StartDate))
                .ForMember(cm => cm.Length,
                    opt => opt.ResolveUsing(cvm => (cvm.EndDate - cvm.StartDate).Days + 1))
                .ForMember(cm => cm.Location,
                    opt => opt.ResolveUsing(cvm => new Location {
                        Address1 = cvm.LocationAddress1,
                        Address2 = cvm.LocationAddress2,
                        Address3 = cvm.LocationAddress3,
                        CityTown = cvm.LocationCityTown,
                        Country = cvm.LocationCountry,
                        PostalCode = cvm.LocationPostalCode,
                        StateProvince = cvm.LocationStateProvince
                    })
                );
        }
    }
}
