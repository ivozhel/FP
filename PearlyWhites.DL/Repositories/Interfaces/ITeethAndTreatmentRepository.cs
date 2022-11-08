namespace PearlyWhites.DL.Repositories.Interfaces
{
    public interface ITeethAndTreatmentRepository
    {
        public Task<bool> DeleteById(int id);
        public Task<bool> Create(int treatmentId, int toothId, string clinicName);
        public Task<IEnumerable<int>> GetTreatmentIdsForTooth(int toothId);
        public Task<IEnumerable<int>> GetTreatmentDayliReport(DateTime date, string clinicName);
    }
}
