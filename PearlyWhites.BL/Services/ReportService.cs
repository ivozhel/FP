using PearlyWhites.BL.Services.Interfaces;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Responses;
using PearlyWhites.Models.Models.Tools;

namespace PearlyWhites.BL.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<BaseResponse<IEnumerable<Report>>> GetAllReportForDate(DateTime from, DateTime to)
        {
            var response = new BaseResponse<IEnumerable<Report>>();
            var reports = await _reportRepository.GetAllReportForDate(from, to);

            if (reports is null)
            {
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.Message = ResponseMessages.SomethingWentWrong;
                response.Respone = null;
                return response;
            }
            response.Message += ResponseMessages.Success;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Respone = reports;
            return response;

        }
    }
}
