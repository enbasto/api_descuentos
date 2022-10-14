using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WSDISCOUNT.Models;

namespace WSDISCOUNT.Services
{
    public class LoginServices
    {
        private readonly IMongoCollection<Users> _ConectCollection;
        public LoginServices(IOptions<DatabaseSettings> DatabaseSettings)
        {
            var mongoClient = new MongoClient(DatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.Value.DatabaseName);
            _ConectCollection = mongoDatabase.GetCollection<Users>(DatabaseSettings.Value.CollectionUsers);
        }
        public async Task<List<Users>> GetUsers() =>
            await _ConectCollection.Find(_ => true).ToListAsync();
        public async Task<Users?> GetUser(string user, string pass)
        {
            // return await _ConectCollection.Find(x => (x.Usuario == user)).FirstOrDefaultAsync();
            var builder = Builders<Users>.Filter;
            var filter = builder.Eq("Usuario", user) & builder.Eq("Password", pass);
            

            return await _ConectCollection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<Users?> GetUserValidation(string id)
        {
            return await _ConectCollection.Find(x => (x.Id == id)).FirstOrDefaultAsync();
        }
    }
}
