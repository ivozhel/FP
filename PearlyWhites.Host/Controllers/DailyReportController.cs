using Microsoft.AspNetCore.Mvc;
using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.BL.Services.Producers;

namespace PearlyWhites.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DailyReportController : ControllerBase
    {
        private readonly IReportProducer _reportProducer;
        private readonly IReportService _reportService;
        public DailyReportController(IReportProducer reportProducer, IReportService reportService)
        {
            _reportProducer = reportProducer;
            _reportService = reportService;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("SendReport")]
        public async Task<IActionResult> Send(DateTime date)
        {
            var location = new Uri($"{Request.Scheme}://{Request.Host}");
            var address = location.AbsoluteUri;
            var result = await _reportProducer.DailyReport(date, address);
            return StatusCode((int)result.StatusCode, result.Respone);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("GetReports")]
        public async Task<IActionResult> GetAllForDate(DateTime? from, DateTime? to)
        {
            var fromDate = from ?? DateTime.Now.AddYears(-100);
            var toDate = to ?? DateTime.Now;

            var result = await _reportService.GetAllReportForDate(fromDate, toDate);
            return StatusCode((int)result.StatusCode, new { result.Respone, result.Message });
        }

    }
}
