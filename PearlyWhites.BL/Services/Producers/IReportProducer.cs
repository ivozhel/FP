using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.BL.Services.Producers
{
    public interface IReportProducer
    {
        public Task<BaseResponse<string>> DailyReport(DateTime date, string address);
    }
}
