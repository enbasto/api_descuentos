using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WSDISCOUNT.Models;

namespace WSDISCOUNT.Services
{
    public class SalesServices
    {
        private readonly IMongoCollection<Sales> _ConectCollection;
        public SalesServices(IOptions<DatabaseSettings> DatabaseSettings)
        {
            var mongoClient = new MongoClient(DatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.Value.DatabaseName);
            _ConectCollection = mongoDatabase.GetCollection<Sales>(DatabaseSettings.Value.CollectionSales);
        }
        public async Task<List<Sales>> GetSales()=>
            await _ConectCollection.Find(_ => true).ToListAsync();
        public async Task CreateSalesAsync(Sales newSales)=>
            await _ConectCollection.InsertOneAsync(newSales);
       
    }
}
