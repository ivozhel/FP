using MediatR;
using Microsoft.AspNetCore.Mvc;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;
using PearlyWhites.Models.Models.Requests.Visit;

namespace PearlyWhites.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TreatmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TreatmentController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetTreatments")]
        public async Task<IActionResult> Get()
        {
            var response = await _mediator.Send(new GetAllTretmentsCommand());
            return StatusCode((int)response.StatusCode, new { response.Respone, response.Message });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TreatmentRequest treatment)
        {
            var response = await _mediator.Send(new AddTreatmentCommand(treatment));
            return StatusCode((int)response.StatusCode, new { response.Respone, response.Message });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("ByID")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetByIdTreatmentCommand(id));
            return StatusCode((int)response.StatusCode, new { response.Respone, response.Message });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Treatment treatment)
        {
            var response = await _mediator.Send(new UpdateTreatmentCommand(treatment));
            return StatusCode((int)response.StatusCode, new { response.Respone, response.Message });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteTreatmentCommand(id));
            return StatusCode((int)response.StatusCode, new { response.Message });

        }
    }
}
