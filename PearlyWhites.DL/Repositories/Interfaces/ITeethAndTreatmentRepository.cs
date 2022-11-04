namespace PearlyWhites.DL.Repositories.Interfaces
{
    public interface ITeethAndTreatmentRepository
    {
        public Task<bool> DeleteById(int id);
        public Task<bool> Create(int treatmentId,int toothId);
        public Task<IEnumerable<int>> GetTreatmentIdsForTooth(int toothId);
    }
}
