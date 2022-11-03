using PearlyWhites.Models.Models;

namespace PearlyWhites.DL.Repositories.Interfaces
{
    public interface ITreatmentsRepository
    {
        public Task<Treatment> GetTreatmentById(int id);
        public Task<IEnumerable<Treatment>> GetAllTreatments();
        public Task<Treatment> Create(Treatment treatment);
        public Task<Treatment> UpdateTreatment(Treatment treatment);
        public Task DeleteTreatmentById(int id);
    }
}
