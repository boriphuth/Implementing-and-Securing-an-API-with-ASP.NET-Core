using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCodeCamp.Data;
using MyCodeCamp.Data.Entities;
using MyCodeCamp.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            try {
                var speakers = _campRepository.GetSpeakersByMoniker(moniker);
                return Ok(_mapper.Map<IEnumerable<SpeakerViewModel>>(speakers));
            } catch(Exception ex) {
                _logger.LogError($"Could nog get speaker: {ex}");
            }

            return BadRequest($"Could nog get speaker");
        }

        [HttpGet("{id}", Name = "GetSpeaker")]
        public IActionResult Get(string moniker, int id) {
            try {
                var speaker = _campRepository.GetSpeaker(id);
                if(speaker == null) {
                    return NotFound();
                }
                if(speaker.Camp.Moniker != moniker) {
                    return BadRequest("Speaker not in specified camp!");
                }
                return Ok(_mapper.Map<SpeakerViewModel>(speaker));
            } catch(Exception ex) {
                _logger.LogError($"Could nog get speaker: {ex}");
            }

            return BadRequest($"Could nog get speaker");
        }

        [HttpPost]
        public async Task<IActionResult> Post(string moniker, [FromBody] SpeakerViewModel viewModel) {
            try {
                var camp = _campRepository.GetCampByMoniker(moniker);
                if(camp == null) {
                    return BadRequest("Could not get camp");
                }

                var speaker = _mapper.Map<Speaker>(viewModel);
                speaker.Camp = camp;
                _campRepository.Add(speaker);

                if(await _campRepository.SaveAllAsync()) {
                    var url = Url.Link("GetSpeaker", new { moniker = camp.Moniker, id = speaker.Id });
                    return Created(url, _mapper.Map<SpeakerViewModel>(speaker));
                }
            } catch(Exception ex) {
                _logger.LogCritical($"Exception thrown while saving a speaker: {ex}.");
            }

            return BadRequest("Could not add new speaker.");
        }
    }
}
