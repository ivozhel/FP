using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Requests;

namespace PearlyWhites.BL.Services.Interfaces
{
    public interface IPatientService
    {
        public Task<Patient> GetPatientById(int id);
        public Task<IEnumerable<Patient>> GetAllPatients();
        public Task<Patient> Create(PatientRequest patient);
        public Task<Patient> UpdatePatient(Patient patient);
        public Task DeletePatientById(int id);
    }
}
