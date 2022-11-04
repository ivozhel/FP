using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Requests;

namespace PearlyWhites.DL.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        public Task<Patient> GetPatientById (int id);
        public Task<IEnumerable<Patient>> GetAllPatients();
        public Task<Patient> Create (Patient patient);    
        public Task<Patient> UpdatePatient (Patient patient);
        public Task<bool> DeletePatientById (int id);
    }
}
