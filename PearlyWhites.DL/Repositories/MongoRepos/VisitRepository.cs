using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PearlyWhites.DL.Repositories.MongoRepos.Interfaces;
using PearlyWhites.Models.Models.Configurations;
using PearlyWhites.Models.Models.Mongo;

namespace PearlyWhites.DL.Repositories.MongoRepos
{
    public class VisitRepository : IVisitRepository
    {
        private readonly IMongoCollection<Visit> _mongoCollection;
        private readonly IOptions<MongoDBVisitsConfiguration> _mongoConfig;

        public VisitRepository(IOptions<MongoDBVisitsConfiguration> mongoConfig)
        {
            _mongoConfig = mongoConfig;
            var mongoClient = new MongoClient(_mongoConfig.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(_mongoConfig.Value.DatabaseName);
            _mongoCollection = mongoDatabase.GetCollection<Visit>(
                _mongoConfig.Value.CollectionName);
        }

        public async Task<Visit> Create(Visit visit)
        {
            await _mongoCollection.InsertOneAsync(visit);
            var created = await _mongoCollection.FindAsync(x => x.Id == visit.Id);
            return created.FirstOrDefault();
        }

        public async Task<IEnumerable<Visit>> GetAll()
        {
            var result = await _mongoCollection.FindAsync(_ => true);
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<Visit>> GetPatientVisits(int patientId)
        {
            var result = await _mongoCollection.FindAsync(x => x.PatientId == patientId);
            return await result.ToListAsync();
        }

        public async Task<Visit> GetVisitById(Guid id)
        {
            var result = await _mongoCollection.FindAsync(x => x.Id == id);
            return result.FirstOrDefault();
        }

        public async Task<Visit> UpdateVisit(Visit visit)
        {
            var update = Builders<Visit>.Update
                .Set(p => p.VisitTreatments, visit.VisitTreatments)
                .Set(p => p.PatientId, visit.PatientId);
            return await _mongoCollection.FindOneAndUpdateAsync(x => x.Id == visit.Id, update);
        }
        public async Task<Visit> PayVisit(Visit visit)
        {
            var update = Builders<Visit>.Update
                .Set(p => p.Paid, visit.Paid);
            return await _mongoCollection.FindOneAndUpdateAsync(x => x.Id == visit.Id, update);
        }

        public async Task Delete(Guid id)
        {
            await _mongoCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Visit>> GetAllOnDate(DateTime date, string clinicName)
        {
            var result = await _mongoCollection.FindAsync(x => x.VisitDate >= date && x.VisitDate <= date.AddHours(23).AddMinutes(59) && x.ClinicName == clinicName);
            return await result.ToListAsync();
        }

    }
}
