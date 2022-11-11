using PearlyWhites.Models.Models.Mongo;

namespace PearlyWhites.DL.Repositories.MongoRepos.Interfaces
{
    public interface IVisitRepository
    {
        public Task<Visit> GetVisitById(Guid id);
        public Task<Visit> Create(Visit visit);
        public Task<Visit> UpdateVisit(Visit visit);
        public Task<IEnumerable<Visit>> GetPatientVisits(int patientId);
        public Task<IEnumerable<Visit>> GetAll();
        public Task<IEnumerable<Visit>> GetAllOnDate(DateTime date, string clinicName);
        public Task<Visit> PayVisit(Visit visit);
        public Task Delete(Guid id);

    }
}
