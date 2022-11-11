using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Requests.Visit;

namespace PearlyWhites.DL.Repositories
{
    public class TreatmentsRepository : ITreatmentsRepository
    {
        private ILogger<TreatmentsRepository> _logger;
        private readonly IConfiguration _configuration;

        public TreatmentsRepository(ILogger<TreatmentsRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<Treatment> Create(Treatment treatment)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var created = await conn.QueryFirstOrDefaultAsync<Treatment>("INSERT INTO Treatments output INSERTED.* VALUES (@Name, @Price, @Description, @IsDeleted)",
                        new { Name = treatment.Name, Price = treatment.Price, Description = treatment.Description, IsDeleted = 0 });
                    return created;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(Create)} : {e.Message}");
            }
            return null;
        }

        public async Task<bool> DeleteTreatmentById(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = $"UPDATE Treatments SET IsDeleted = 1 WHERE Id = {id}";
                    await conn.OpenAsync();

                    await conn.QueryAsync(query);
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeleteTreatmentById)} : {e.Message}");
            }

            return false;
        }

        public async Task<IEnumerable<Treatment>> GetAllTreatments()
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM Treatments WITH(NOLOCK) WHERE IsDeleted = 0";
                    await conn.OpenAsync();
                    var treatments = await conn.QueryAsync<Treatment>(query);
                    return treatments;

                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllTreatments)} : {e.Message}");
            }

            return Enumerable.Empty<Treatment>();
        }

        public async Task<Treatment> GetTreatmentById(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM Treatments WITH(NOLOCK) WHERE Id = @Id AND IsDeleted = 0";
                    await conn.OpenAsync();
                    var treatment = await conn.QueryFirstOrDefaultAsync<Treatment>(query, new { Id = id });
                    return treatment;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetTreatmentById)} : {e.Message}");
            }

            return null;
        }

        public async Task<Treatment> UpdateTreatment(Treatment treatment)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var updatedPatient = await conn.QueryFirstOrDefaultAsync<Treatment>("UPDATE Treatments SET Name = @Name, Price = @Price, Description = @Description output INSERTED.* WHERE Id = @Id AND IsDeleted = 0",
                        new { Id = treatment.Id, Name = treatment.Name, Price = treatment.Price, Description = treatment.Description });
                    return updatedPatient;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdateTreatment)} : {e.Message}");
            }
            return null;
        }

        public async Task<bool> CheckIfTreatmentExists(TreatmentRequest treatmentRequest)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM Treatments WITH(NOLOCK) WHERE [Name] = @Name AND IsDeleted = 0";
                    await conn.OpenAsync();
                    var treatment = await conn.QueryFirstOrDefaultAsync<Treatment>(query, new { Name = treatmentRequest.Name });

                    if (treatment is null)
                        return false;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetTreatmentById)} : {e.Message}");
            }

            return true;
        }
    }
}
