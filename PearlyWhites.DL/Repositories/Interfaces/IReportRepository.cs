using PearlyWhites.Models.Models;

namespace PearlyWhites.DL.Repositories.Interfaces
{
    public interface IReportRepository
    {
        public Task<Report> AddReport(Report report);
        public Task<bool> CheckIfExists(string name, DateTime date);
        public Task<IEnumerable<Report>> GetAllReportForDate(DateTime from, DateTime to);
    }
}
