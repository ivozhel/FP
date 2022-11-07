using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;

namespace PearlyWhites.DL.Repositories
{
    public class TeethAndTreatmentRepository : ITeethAndTreatmentRepository
    {
        private ILogger<PatientRepository> _logger;
        private readonly IConfiguration _configuration;

        public TeethAndTreatmentRepository(ILogger<PatientRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = $"UPDATE Teeth_Treatments SET IsDeleted = 1 WHERE Id = {id}";
                    await conn.OpenAsync();

                    await conn.QueryAsync(query);
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeleteById)} : {e.Message}");
            }

            return false;
        }

        public async Task<bool> Create(int treatmentId, int toothId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var created = await conn.QueryFirstOrDefaultAsync<Treatment>("INSERT INTO Teeth_Treatments output INSERTED.* VALUES (@ToothId, @TreatmentId,@IsDeleted, @Date)",
                        new { ToothId = toothId, TreatmentId = treatmentId, IsDeleted = 0, Date = DateTime.Now });
                    if (created is not null)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(Create)} : {e.Message}");
            }
            return false;
        }

        public async Task<IEnumerable<int>> GetTreatmentIdsForTooth(int toothId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT TreatmentId FROM Teeth_Treatments WITH(NOLOCK) WHERE ToothId = @ToothId";
                    await conn.OpenAsync();
                    var result = await conn.QueryAsync<int>(query, new { ToothId = toothId });
                    return result;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetTreatmentIdsForTooth)} : {e.Message}");
            }
            return null;
        }
    }
}
