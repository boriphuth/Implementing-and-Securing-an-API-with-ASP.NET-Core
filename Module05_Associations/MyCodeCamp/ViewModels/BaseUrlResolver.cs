using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Controllers;
using System;

namespace MyCodeCamp.ViewModels
{
    public abstract class BaseUrlResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;        

        public BaseUrlResolver(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context) {
            var url = _httpContextAccessor.HttpContext.Items[BaseController.URLHELPER] as IUrlHelper;
            return CreateUrl(source, destination, destMember, context, url);
        }

        protected abstract string CreateUrl(TSource source, TDestination desintation, string destMember, ResolutionContext context, IUrlHelper urlHelper);
    }
}
