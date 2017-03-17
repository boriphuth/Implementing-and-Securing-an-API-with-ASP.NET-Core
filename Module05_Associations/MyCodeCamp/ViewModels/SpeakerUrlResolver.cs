using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Data.Entities;

namespace MyCodeCamp.ViewModels
{
    public class SpeakerUrlResolver : BaseUrlResolver<Speaker, SpeakerViewModel>
    {
        public SpeakerUrlResolver(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor) { }

        protected override string CreateUrl(Speaker source, SpeakerViewModel destination, string destMember, ResolutionContext context, IUrlHelper urlHelper) {
            return urlHelper.Link("GetSpeaker", new { moniker = source.Camp.Moniker, id = source.Id });
        }
    }
}
