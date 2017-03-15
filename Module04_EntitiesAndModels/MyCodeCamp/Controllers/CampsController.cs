using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCodeCamp.Data;
using MyCodeCamp.Data.Entities;
using MyCodeCamp.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyCodeCamp.Controllers {     
    [Route("api/[controller]")]
	public class CampsController : BaseController {
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
                // return Ok(_mapper.Map<IEnumerable<CampViewModel>>(camps, opt => opt.Items["UrlHelper"] = Url)); /* => this method was cumbersome to use, now implemented by custom resolver */
                return Ok(_mapper.Map<IEnumerable<CampViewModel>>(camps));
            } catch (Exception ex) {
                _logger.LogCritical($"Threw exception while getting camps: {ex}");
            }

            return BadRequest("Could not get camps");
        }
        
        [HttpGet("{moniker}", Name = "GetCamp")]
        public IActionResult Get(string moniker, bool includeSpeakers = false) {
            try {
                var camp = includeSpeakers
                    ? _campRepository.GetCampByMonikerWithSpeakers(moniker)
                    : _campRepository.GetCampByMoniker(moniker);
                if (camp == null) {
                    return NotFound($"Camp with moniker '{moniker}' was not found.");
                }
                // return Ok(_mapper.Map<CampViewModel>(camp, opt => opt.Items["UrlHelper"] = Url)); /* => this method was cumbersome to use, now implemented by custom resolver */
                return Ok(_mapper.Map<CampViewModel>(camp));
            } catch (Exception ex) {
                _logger.LogCritical($"Threw exception while getting camp with moniker='{moniker}' and includeSpeakers='{includeSpeakers}': {ex}");
            }

            return BadRequest("Could not get camp");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CampViewModel viewModel) {
            try {

                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Create a new code camp");

                var camp = _mapper.Map<Camp>(viewModel);
                _campRepository.Add(camp);
                if(await _campRepository.SaveAllAsync()) {
                    var uri = Url.Link("GetCamp", new { moniker = camp.Moniker });
                    return Created(uri, _mapper.Map<CampViewModel>(camp));
                } else {
                    _logger.LogWarning("Could not save camp to the database");
                }
            } catch (Exception ex) {
                _logger.LogCritical($"Threw exception while saving camp: {ex}");
            }

            return BadRequest("Could not save camp");
        }

        [HttpPut("{moniker}")]
        [HttpPatch("{moniker}")]
        public async Task<IActionResult> Put(string moniker, [FromBody] CampViewModel viewModel) {
            try {
                _logger.LogInformation($"Put code camp with moniker='{moniker}'");

                var camp = _campRepository.GetCampByMoniker(moniker);
                if(camp == null) {
                    return NotFound($"Could not find a camp with moniker='{moniker}'");
                }                

                /*camp.Name = viewModel.Name ?? camp.Name;
                camp.Description = viewModel.Description ?? camp.Description;
                camp.Location = new Location {
                    Address1 = viewModel.LocationAddress1,
                    Address2 = viewModel.LocationAddress2,
                    Address3 = viewModel.LocationAddress3,
                    CityTown = viewModel.LocationCityTown,
                    Country = viewModel.LocationCountry,
                    PostalCode = viewModel.LocationPostalCode,
                    StateProvince = viewModel.LocationStateProvince
                };
                camp.Length = viewModel.Length > 0 ? viewModel.Length : camp.Length;
                camp.EventDate = viewModel.EventDate != DateTime.MinValue ? viewModel.EventDate : camp.EventDate;*/

                if (await _campRepository.SaveAllAsync()) {                    
                    return Ok(_mapper.Map<CampViewModel>(camp));
                } else {
                    _logger.LogWarning("Could not save camp to the database");
                }
            } catch (Exception ex) {
                _logger.LogCritical($"Threw exception while saving camp: {ex}");
            }

            return BadRequest("Could not save camp");
        }

        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete(string moniker) {
            try {
                _logger.LogInformation($"Delete code camp with moniker='{moniker}'");

                var camp = _campRepository.GetCampByMoniker(moniker);
                if (camp == null) {
                    return NotFound($"Could not find a camp with moniker='{moniker}'");
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