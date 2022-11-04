using MediatR;
using Microsoft.AspNetCore.Mvc;
using PearlyWhites.Host.Extensions.SwaggerExamples;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.MediatRCommands.Treatments;
using PearlyWhites.Models.Models.Requests;
using Swashbuckle.AspNetCore.Filters;

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
            return Ok(await _mediator.Send(new GetAllTretmentsCommand()));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TreatmentRequest treatment)
        {
            var created = await _mediator.Send(new AddTreatmentCommand(treatment));
            if (created is not null)
            {
                return Ok(created);
            }
            return BadRequest();
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
            var treatment = await _mediator.Send(new GetByIdTreatmentCommand(id));
            if (treatment is not null)
            {
                return Ok(treatment);
            }
            else
            {
                return NotFound("Treatment with this id dose not exist");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Treatment treatment)
        {
            var toUp = await _mediator.Send(new GetByIdTreatmentCommand(treatment.Id));
            if (toUp is null)
            {
                return NotFound("Treatment with this id dose not exist");
            }

            return Ok(await _mediator.Send(new UpdateTreatmentCommand(treatment)));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var toDel = await _mediator.Send(new GetByIdTreatmentCommand(id));
            if (toDel is null)
            {
                return NotFound("Treatment with this id dose not exist");
            }
            var isDeleted = await _mediator.Send(new DeleteTreatmentCommand(id));
            if (isDeleted)
            {
                return Ok("Succsesfully deleted treatment");
            }
            return BadRequest();
        }
    }
}
