using WSDISCOUNT.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace WSDISCOUNT.Services
{
    public class DiscountServices
    {
        private readonly IMongoCollection<ObjetDescuento> _ConectCollection;
        public DiscountServices(IOptions<DatabaseSettings> DatabaseSettings)
        {
            var mongoClient = new MongoClient(DatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.Value.DatabaseName);
            _ConectCollection = mongoDatabase.GetCollection<ObjetDescuento>(DatabaseSettings.Value.CollectionDiscount);
        }
        public async Task<ObjetDescuento?> PostDiscount(string console) =>
            await _ConectCollection.Find(x => x.Console == console).FirstOrDefaultAsync();

    }
}
