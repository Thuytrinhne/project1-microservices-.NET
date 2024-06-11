using Microsoft.Extensions.Options;
using MongoDB.Driver;
using User.API.Settings;

namespace User.API.Service
{
    public class EmailVerificationService
    {
            private readonly IMongoCollection<EmailVerification>  _collection;
           private readonly MongoDbConfig _mongoConfig;

        public EmailVerificationService(
                IOptions<MongoDbConfig> mongoConfig)
            {
                 _mongoConfig = mongoConfig.Value;
                var mongoClient = new MongoClient(
                    _mongoConfig.ConnectionString);

                var mongoDatabase = mongoClient.GetDatabase(
                    _mongoConfig.Name);

                _collection = mongoDatabase.GetCollection<EmailVerification>(
                    _mongoConfig.EmailVerificationCollectionName);
            }

    
            public async Task<EmailVerification?> GetByEmailAsync(string email) =>
                await _collection.Find(x => x.Email == email)
            .SortByDescending(x => x.ExpiryTime)
            .FirstOrDefaultAsync();

            public async Task CreateAsync(EmailVerification emailVerification) =>
                await _collection.InsertOneAsync(emailVerification);
            public async Task RemoveAsync(string id) =>
                await _collection.DeleteOneAsync(x => x.Id == id);
        }
    }
