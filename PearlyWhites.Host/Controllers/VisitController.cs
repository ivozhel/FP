using Microsoft.AspNetCore.Mvc;
using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.Models.Models.Requests.Visit;

namespace PearlyWhites.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VisitController : ControllerBase
    {
        private readonly IVisitService _visitService;

        public VisitController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(VisitRequest visit)
        {
            var location = new Uri($"{Request.Scheme}://{Request.Host}");
            var clinicName = location.AbsoluteUri;
            var result = await _visitService.Create(visit, clinicName);
            return StatusCode((int)result.StatusCode, new { result.Respone, result.Message });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetById")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _visitService.GetVisitById(id);
            return StatusCode((int)result.StatusCode, new { result.Respone, result.Message });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            var result = await _visitService.GetAll();
            return StatusCode((int)result.StatusCode, new { result.Respone, result.Message });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetAllForPatient")]
        public async Task<IActionResult> GetAllForPatient(int patientId)
        {
            var result = await _visitService.GetPatientVisits(patientId);
            return StatusCode((int)result.StatusCode, new { result.Respone, result.Message });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("PayVisit")]
        public async Task<IActionResult> PayVisit(Guid visiId, decimal money)
        {
            var result = await _visitService.PayVisit(visiId, money);
            return StatusCode((int)result.StatusCode, new { result.Respone, result.Message });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Guid visiId)
        {
            await _visitService.Delete(visiId);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPut("UpdateVisit")]
        public async Task<IActionResult> UpdateVisit(VisitUpdateRequest visitUpdate)
        {
            var result = await _visitService.UpdateVisit(visitUpdate);
            return StatusCode((int)result.StatusCode, new { result.Respone, result.Message });
        }
    }
}
