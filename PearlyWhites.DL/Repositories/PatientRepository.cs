using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PearlyWhites.DL.Repositories.Interfaces;
using PearlyWhites.Models.Models;

namespace PearlyWhites.DL.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private ILogger<PatientRepository> _logger;
        private readonly IConfiguration _configuration;

        public PatientRepository(ILogger<PatientRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Patient>> GetAllPatients()
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM Patients WITH(NOLOCK) WHERE IsDeleted = 0";
                    await conn.OpenAsync();
                    var patients = await conn.QueryAsync<Patient>(query);
                    return patients;

                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetAllPatients)} : {e.Message}");
            }

            return Enumerable.Empty<Patient>();
        }

        public async Task<Patient> GetPatientById(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT * FROM Patients WITH(NOLOCK) WHERE Id = @Id AND IsDeleted = 0";
                    await conn.OpenAsync();
                    var patient = await conn.QueryFirstOrDefaultAsync<Patient>(query, new { Id = id });
                    return patient;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(GetPatientById)} : {e.Message}");
            }

            return null;
        }

        public async Task<bool> DeletePatientById(int id)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = $"UPDATE Patients SET IsDeleted = 1 WHERE Id = {id} AND IsDeleted = 0";
                    await conn.OpenAsync();

                    await conn.QueryAsync(query);
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(DeletePatientById)} : {e.Message}");
            }

            return false;
        }

        public async Task<Patient> UpdatePatient(Patient patient)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var updatedPatient = await conn.QueryFirstOrDefaultAsync<Patient>("UPDATE Patients SET Name = @Name, Age = @Age output INSERTED.* WHERE Id = @Id AND IsDeleted = 0",
                        new { Name = patient.Name, Age = patient.Age, Id = patient.Id });
                    return updatedPatient;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(UpdatePatient)} : {e.Message}");
            }
            return null;

        }

        public async Task<Patient> Create(Patient patient)
        {
            try
            {
                await using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await conn.OpenAsync();
                    var created = await conn.QueryFirstOrDefaultAsync<Patient>("INSERT INTO Patients output INSERTED.* VALUES (@Name, @Age,@IsDeleted)",
                        new { Name = patient.Name, Age = patient.Age, IsDeleted = 0 });
                    return created;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in {nameof(Create)} : {e.Message}");
            }
            return null;
        }

    }
}
