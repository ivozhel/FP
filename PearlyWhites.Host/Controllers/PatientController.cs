using Microsoft.AspNetCore.Mvc;
using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.Host.Extensions.SwaggerExamples;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Requests;
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
        [HttpGet("GetPatients")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _patientService.GetAllPatients());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("ByID")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var patient = await _patientService.GetPatientById(id); 
            if (patient is not null)
            {
                return Ok(patient);
            }
            else
            {
                return NotFound("Patient with this id dose not exist");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(PatientRequest),typeof(PatientSwaggerExample))]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientRequest patient)
        {
            var created = await _patientService.Create(patient);
            if (created is not null)
            {
                return Ok(created);
            }
            return BadRequest();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var toDelete = await _patientService.GetPatientById(id);
            if (toDelete is null)
            {
                return NotFound("Patient with this id dose not exist");
            }
            await _patientService.DeletePatientById(id);
            return Ok("Deleted succsesfully");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(Patient), typeof(PatientToUpdateSwaggerExample))] 
        public async Task<IActionResult> Update(Patient patient)
        {
            var toUp = await _patientService.GetPatientById(patient.Id);
            if (toUp is null)
            {
                return NotFound("Patient with this id dose not exist");
            }

            return Ok(await _patientService.UpdatePatient(patient));
        }
    }
}