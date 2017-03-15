﻿using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Data;

namespace MyCodeCamp.Controllers {     
    [Route("api/[controller]")]
	public class CampsController : Controller {
        private readonly ICampRepository _campRepository;

        public CampsController(ICampRepository campRepository) {
            _campRepository = campRepository;
        }

        [HttpGet]
        public IActionResult Get() {
            try {
                var camps = _campRepository.GetAllCamps();
                return Ok(camps);
            } catch { }

            return BadRequest();
        }

        [HttpGet("")]
        [HttpGet("{id}")]
        public IActionResult Get(int id, bool includeSpeakers = false) {
            try {
                var camp = includeSpeakers
                    ? _campRepository.GetCampWithSpeakers(id)
                    : _campRepository.GetCamp(id);
                if (camp == null) {
                    return NotFound($"Camp with id '{id}' was not found.");
                }
                return Ok(camp);
            } catch { }

            return BadRequest();
        }
    }
}