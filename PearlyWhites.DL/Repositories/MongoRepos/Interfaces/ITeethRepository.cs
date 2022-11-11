using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Mongo;

namespace PearlyWhites.DL.Repositories.MongoRepos.Interfaces
{
    public interface ITeethRepository
    {
        public Task<Tooth> GetToothById(Guid id);
        public Task<Teeth> Create(Teeth teeth);
        public Task<Tooth> UpdateTooth(Tooth tooth);
        public Task<IEnumerable<Tooth>> GetPatientTeeth(int patientId);
        public Task<bool> DeletePatientTeeth(int patientId);
    }
}
