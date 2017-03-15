using Microsoft.AspNetCore.Mvc;
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
            var camps = _campRepository.GetAllCamps();
            return Ok(camps);
        }
    }
}