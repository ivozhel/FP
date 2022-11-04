using Microsoft.AspNetCore.Mvc;
using PearlyWhites.BL.Services.Interfaces;

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
    }
}
