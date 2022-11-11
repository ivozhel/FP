using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PearlyWhites.DL.Repositories.MongoRepos.Interfaces;
using PearlyWhites.Models.Models;
using PearlyWhites.Models.Models.Configurations;
using PearlyWhites.Models.Models.Mongo;

namespace PearlyWhites.DL.Repositories.MongoRepos
{
    public class TeethRepository : ITeethRepository
    {
        private readonly IMongoCollection<Teeth> _mongoCollection;
        private readonly IOptions<MongoDBTeethConfiguration> _mongoConfig;

        public TeethRepository(IOptions<MongoDBTeethConfiguration> mongoConfig)
        {
            _mongoConfig = mongoConfig;
            var mongoClient = new MongoClient(_mongoConfig.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(_mongoConfig.Value.DatabaseName);
            _mongoCollection = mongoDatabase.GetCollection<Teeth>(
                _mongoConfig.Value.CollectionName);
        }

        public async Task<Teeth> Create(Teeth teeth)
        {
            await _mongoCollection.InsertOneAsync(teeth);
            var created = await _mongoCollection.FindAsync(x => x.Id == teeth.Id);
            return created.FirstOrDefault();
        }

        public async Task<bool> DeletePatientTeeth(int patientId)
        {
            return true;
        }

        public async Task<IEnumerable<Tooth>> GetPatientTeeth(int patientId)
        {
            var result = await _mongoCollection.FindAsync(x => x.PatientId == patientId);
            return result.FirstOrDefault().TeethList;
        }

        public async Task<Tooth> GetToothById(Guid id)
        {
            var result = await _mongoCollection.FindAsync(x => x.TeethList.Any(x => x.Id == id));
            var teethList = result.FirstOrDefault();
            if (teethList is null)
            {
                return null;
            }
            return teethList.TeethList.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Tooth> UpdateTooth(Tooth tooth)
        {
            var update = Builders<Teeth>.Update
                .Set(p => p.TeethList.FirstOrDefault(x => x.Id == tooth.Id), tooth);
            if (update is null)
            {
                return null;
            }
            var result = await _mongoCollection.FindOneAndUpdateAsync(x => x.TeethList.Any(x => x.Id == tooth.Id), update);
            return result.TeethList.FirstOrDefault(x => x.Id == tooth.Id);
        }
    }
}
