using Microsoft.AspNetCore.Mvc;
using PearlyWhites.BL.Services.Producers;

namespace PearlyWhites.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DailyReportController : ControllerBase
    {
        private readonly IReportProducer _reportProducer;
        public DailyReportController(IReportProducer reportProducer)
        {
            _reportProducer = reportProducer;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("SendReport")]
        public async Task<IActionResult> Send(DateTime date)
        {            
            return Ok(await _reportProducer.DailyReport(date));
        }

    }
}
