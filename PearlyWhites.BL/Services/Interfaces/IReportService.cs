using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.BL.Services.Interfaces
{
    public interface IReportService
    {
        public Task<BaseResponse<IEnumerable<Report>>> GetAllReportForDate(DateTime from, DateTime to);
    }
}
