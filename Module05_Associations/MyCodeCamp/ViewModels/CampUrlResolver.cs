using AutoMapper;
using MyCodeCamp.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MyCodeCamp.ViewModels
{
    public class CampUrlResolver : BaseUrlResolver<Camp, CampViewModel>
    {        
        public CampUrlResolver(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor) { }

        protected override string CreateUrl(Camp source, CampViewModel destination, string destMember, ResolutionContext context, IUrlHelper urlHelper) {
            return urlHelper.Link("GetCamp", new { moniker = source.Moniker });
        }
    }
}
