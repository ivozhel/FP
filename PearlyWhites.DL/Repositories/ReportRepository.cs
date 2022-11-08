using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;

namespace PearlyWhites.DL.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private ILogger<ReportRepository> _logger;
        private readonly IConfiguration _configuration;

        public ReportRepository(ILogger<ReportRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Report> AddReport(Report report)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var created = await conn.QueryFirstOrDefaultAsync<Report>("INSERT INTO DayliReport output INSERTED.* VALUES (@Name, @Total,@Date)",
                        new { Name = report.Name, Total = report.Total, Date = report.Date });
                    return created;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(AddReport)} : {e.Message}");
            }
            return null;
        }

        public async Task<bool> CheckIfExists(string name, DateTime date)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM DayliReport WITH(NOLOCK) WHERE [Name] = @Name AND cast([Date] as date) = @Date";
                    await conn.OpenAsync();
                    var report = await conn.QueryFirstOrDefaultAsync<Treatment>(query, new { Name = name, Date = date.Date });

                    if (report is null)
                        return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(CheckIfExists)} : {e.Message}");
            }

            return true;
        }
        public async Task<IEnumerable<Report>> GetAllReportForDate(DateTime from, DateTime to)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM DayliReport WITH(NOLOCK) WHERE [Date] BETWEEN @From AND @To";
                    await conn.OpenAsync();
                    var reports = await conn.QueryAsync<Report>(query, new { From = from, To = to });
                    return reports;

                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllReportForDate)} : {e.Message}");
            }

            return Enumerable.Empty<Report>();
        }
    }
}
