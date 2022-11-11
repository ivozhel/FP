using Microsoft.AspNetCore.Mvc;
using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.Host.Extensions.SwaggerExamples;
using PearlyWhites.Models.Models.Requests.Patient;
using Swashbuckle.AspNetCore.Filters;

namespace PearlyWhites.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetPatients")]
        public async Task<IActionResult> Get()
        {
            var response = await _patientService.GetAllPatients();
            return StatusCode((int)response.StatusCode, new { response.Respone, response.Message });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("ByID")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _patientService.GetPatientById(id);
            return StatusCode((int)response.StatusCode, new { response.Respone, response.Message });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(PatientRequest), typeof(PatientSwaggerExample))]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientRequest patient)
        {
            var response = await _patientService.Create(patient);
            return StatusCode((int)response.StatusCode, new { response.Respone, response.Message });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _patientService.Delete(id);
            return StatusCode((int)response.StatusCode, response.Message);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(PatientUpdateRequest patient)
        {
            var response = await _patientService.Update(patient);
            return StatusCode((int)response.StatusCode, response.Message);
        }
    }
}