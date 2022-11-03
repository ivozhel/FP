using PearlyWhites.Models.Models;

namespace PearlyWhites.DL.Repositories.Interfaces
{
    public interface IToothRepository
    {
        public Task<Tooth> GetToothById(int id);
        public Task<Tooth> Create(Tooth tooth);
        public Task<Tooth> UpdateTooth(Tooth tooth);
        public Task<IEnumerable<Tooth>> GetPatientTeeth(int patientId);
        public Task DeletePatientTeeth(int patientId);
    }
}
