using AutoMapper;
using MyCodeCamp.Data.Entities;
using MyCodeCamp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MyCodeCamp.ViewModels
{
    public class CampUrlResolver : IValueResolver<Camp, CampViewModel, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CampUrlResolver(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(Camp source, CampViewModel destination, string destMember, ResolutionContext context) {
            var url = _httpContextAccessor.HttpContext.Items[BaseController.URLHELPER] as IUrlHelper;
            return url.Link("GetCamp", new { id = source.Id });
        }
    }
}
