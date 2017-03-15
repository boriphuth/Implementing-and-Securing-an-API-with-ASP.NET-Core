using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCodeCamp.Data;
using MyCodeCamp.Data.Entities;
using MyCodeCamp.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCodeCamp.Controllers {     
    [Route("api/[controller]")]
	public class CampsController : Controller {
        private readonly ICampRepository _campRepository;
        private readonly ILogger<CampsController> _logger;
        private readonly IMapper _mapper;

        public CampsController(ICampRepository campRepository, 
            ILogger<CampsController> logger, IMapper mapper) {
            _campRepository = campRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get() {
            try {
                var camps = _campRepository.GetAllCamps();
                return Ok(_mapper.Map<IEnumerable<CampViewModel>>(camps, opt => opt.Items["UrlHelper"] = Url));
            } catch (Exception ex) {
                _logger.LogCritical($"Threw exception while getting camps: {ex}");
            }

            return BadRequest("Could not get camps");
        }
        
        [HttpGet("{id}", Name = "GetCamp")]
        public IActionResult Get(int id, bool includeSpeakers = false) {
            try {
                var camp = includeSpeakers
                    ? _campRepository.GetCampWithSpeakers(id)
                    : _campRepository.GetCamp(id);
                if (camp == null) {
                    return NotFound($"Camp with id '{id}' was not found.");
                }
                return Ok(_mapper.Map<CampViewModel>(camp, opt => opt.Items["UrlHelper"] = Url));
            } catch (Exception ex) {
                _logger.LogCritical($"Threw exception while getting camp with id='{id}' and includeSpeakers='{includeSpeakers}': {ex}");
            }

            return BadRequest("Could not get camp");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Camp model) {
            try {
                _logger.LogInformation("Create a new code camp");
                _campRepository.Add(model);                
                if(await _campRepository.SaveAllAsync()) {
                    var uri = Url.Link("GetCamp", new { id = model.Id });
                    return Created(uri, model);
                } else {
                    _logger.LogWarning("Could not save camp to the database");
                }
            } catch (Exception ex) {
                _logger.LogCritical($"Threw exception while saving camp: {ex}");
            }

            return BadRequest("Could not save camp");
        }

        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Camp model) {
            try {
                _logger.LogInformation($"Put code camp with id='{id}'");

                var camp = _campRepository.GetCamp(id);
                if(camp == null) {
                    return NotFound($"Could not find a camp with id='{id}'");
                }

                camp.Name = model.Name ?? camp.Name;
                camp.Description = model.Description ?? camp.Description;
                camp.Location = model.Location ?? camp.Location;
                camp.Length = model.Length > 0 ? model.Length : camp.Length;
                camp.EventDate = model.EventDate != DateTime.MinValue ? model.EventDate : camp.EventDate;

                if (await _campRepository.SaveAllAsync()) {                    
                    return Ok(camp);
                } else {
                    _logger.LogWarning("Could not save camp to the database");
                }
            } catch (Exception ex) {
                _logger.LogCritical($"Threw exception while saving camp: {ex}");
            }

            return BadRequest("Could not save camp");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            try {
                _logger.LogInformation($"Delete code camp with id='{id}'");

                var camp = _campRepository.GetCamp(id);
                if (camp == null) {
                    return NotFound($"Could not find a camp with id='{id}'");
                }

                _campRepository.Delete(camp);
                if (await _campRepository.SaveAllAsync()) {
                    return Ok();
                } else {
                    _logger.LogWarning("Could not delete camp to the database");
                }
            } catch (Exception ex) {
                _logger.LogCritical($"Threw exception while deleting camp: {ex}");
            }

            return BadRequest("Could not delete camp");
        }
    }
}