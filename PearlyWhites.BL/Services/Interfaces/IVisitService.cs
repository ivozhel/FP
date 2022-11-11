using PearlyWhites.Models.Models.Mongo;
using PearlyWhites.Models.Models.Requests.Visit;
using PearlyWhites.Models.Models.Responses;

namespace PearlyWhites.BL.Services.Interfaces
{
    public interface IVisitService
    {
        public Task<BaseResponse<Visit>> GetVisitById(Guid id);
        public Task<BaseResponse<Visit>> Create(VisitRequest visit, string clinicName);
        public Task<BaseResponse<Visit>> UpdateVisit(VisitUpdateRequest visit);
        public Task<BaseResponse<IEnumerable<Visit>>> GetPatientVisits(int patientId);
        public Task<BaseResponse<IEnumerable<Visit>>> GetAll();
        public Task<BaseResponse<decimal>> PayVisit(Guid visitId, decimal money);
        public Task Delete(Guid id);
    }
}
