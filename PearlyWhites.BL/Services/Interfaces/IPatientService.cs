using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Requests.Patient;
using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.BL.Services.Interfaces
{
    public interface IPatientService
    {
        public Task<BaseResponse<Patient>> GetPatientById(int id);
        public Task<BaseResponse<IEnumerable<Patient>>> GetAllPatients();
        public Task<BaseResponse<Patient>> Create(PatientRequest patient);
        public Task<BaseResponse<Patient>> UpdatePatient(PatientUpdateRequest patientReq);
        public Task<BaseResponse<bool>> DeletePatientById(int id);
    }
}
