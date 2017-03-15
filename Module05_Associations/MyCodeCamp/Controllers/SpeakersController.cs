using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCodeCamp.Data;

namespace MyCodeCamp.Controllers
{
    [Route("api/camps/{moniker}/speakers")]
    public class SpeakersController : BaseController
    {
        private readonly ICampRepository _campRepository;
        private readonly ILogger<SpeakersController> _logger;
        private readonly IMapper _mapper;

        public SpeakersController(ICampRepository campRepository,
            ILogger<SpeakersController> logger, IMapper mapper) {
            _campRepository = campRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(string moniker) {
            var speakers = _campRepository.GetSpeakersByMoniker(moniker);
            return Ok(speakers);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string moniker, int id)
        {
            var speaker = _campRepository.GetSpeaker(id);
            if(speaker == null) {
                return NotFound();
            }
            if(speaker.Camp.Moniker != moniker) {
                return BadRequest("Speaker not in specified camp!");
            }
            return Ok(speaker);
        }
    }
}
