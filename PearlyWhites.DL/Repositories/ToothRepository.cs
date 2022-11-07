using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;
using Microsoft.Extensions.Logging;
using Dapper;

namespace PearlyWhites.DL.Repositories
{
    public class ToothRepository : IToothRepository
    {
        private ILogger<PatientRepository> _logger;
        private readonly IConfiguration _configuration;

        public ToothRepository(IConfiguration configuration, ILogger<PatientRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Tooth> Create(Tooth tooth)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var created = await conn.QueryFirstOrDefaultAsync<Tooth>("INSERT INTO Theet output INSERTED.* VALUES (@Name, @Position,@PatientId,@IsDeleted)",
                        new { Name = tooth.Name, Position = tooth.Position, PatientId = tooth.PatientId, IsDeleted = 0});
                    return created;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(Create)} : {e.Message}");
            }
            return null;
        }

        public async Task<Tooth> GetToothById(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM Theet WITH(NOLOCK) WHERE Id = @Id AND IsDeleted = 0";
                    await conn.OpenAsync();
                    var tooth = await conn.QueryFirstOrDefaultAsync<Tooth>(query, new { Id = id });
                    return tooth;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetToothById)} : {e.Message}");
            }

            return null;
        }

        public async Task<Tooth> UpdateTooth(Tooth tooth)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var updatedTooth = await conn.QueryFirstOrDefaultAsync<Tooth>("UPDATE Theet SET Name = @Name, Position = @Position output INSERTED.* WHERE Id = @Id AND IsDeleted = 0",
                        new { Name = tooth.Name, Positin = tooth.Position});
                    return updatedTooth;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdateTooth)} : {e.Message}");
            }
            return null;

        }

        public async Task<IEnumerable<Tooth>> GetPatientTeeth(int patientId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM Theet WITH(NOLOCK) WHERE PatientId = @PatientId AND IsDeleted = 0";
                    await conn.OpenAsync();

                    return await conn.QueryAsync<Tooth>(query, new { PatientId = patientId });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetPatientTeeth)} : {e.Message}");
            }
            return null;
        }
        public async Task<bool> DeletePatientTeeth(int patientId)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "UPDATE Theet SET IsDeleted = 1 WHERE PatientId = @patientId";
                    await conn.OpenAsync();

                    await conn.QueryAsync(query, new { patientId = patientId});
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeletePatientTeeth)} : {e.Message}");
            }

            return false;
        }
    }
}
