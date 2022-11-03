using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using Microsoft.Extensions.Logging;
using Dapper;

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
                    var created = await conn.QueryFirstOrDefaultAsync<Treatment>("INSERT INTO Treatments output INSERTED.* VALUES (@Name, @Price, @Description,@LastUpdated)",
                        new { Name = treatment.Name, Price = treatment.Price, Description = treatment.Description, LastUpdated = DateTime.Now });
                    return created;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(Create)} : {e.Message}");
            }
            return null;
        }

        public async Task DeleteTreatmentById(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = $"DELETE FROM Treatments WHERE Id = {id}";
                    await conn.OpenAsync();

                    await conn.QueryAsync(query);
                    return;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeleteTreatmentById)} : {e.Message}");
            }

            return;
        }

        public async Task<IEnumerable<Treatment>> GetAllTreatments()
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM Treatments WITH(NOLOCK)";
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
                    var query = "SELECT * FROM Treatments WITH(NOLOCK) WHERE Id = @Id";
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
                    var updatedPatient = await conn.QueryFirstOrDefaultAsync<Treatment>("UPDATE Treatments SET Name = @Name, Price = @Price, Description = @Description, LastUpdated = @LastUpdated  output INSERTED.* WHERE Id = @Id",
                        new {Id = treatment.Id, Name = treatment.Name, Price = treatment.Price, Description = treatment.Description, LastUpdated = DateTime.Now });
                    return updatedPatient;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdateTreatment)} : {e.Message}");
            }
            return null;
        }
    }
}
