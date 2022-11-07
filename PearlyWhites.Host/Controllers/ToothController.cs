using Microsoft.AspNetCore.Mvc;
using PearlyWhites.BL.Services;
using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.Models.Models.Requests.Patient;
using PearlyWhites.Models.Models.Requests.Tooth;

namespace PearlyWhites.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToothController : ControllerBase
    {
        private readonly IToothService _toothService;

        public ToothController(IToothService toothService)
        {
            _toothService = toothService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("ByID")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _toothService.GetToothById(id);

            return StatusCode((int)response.StatusCode, new { response.Respone, response.Message });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(ToothUpdateRequest tooth)
        {
            var response = await _toothService.UpdateTooth(tooth);
            return StatusCode((int)response.StatusCode, response.Message);
        }
    }
}
