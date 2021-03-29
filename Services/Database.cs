using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PersonalCrm
{
    public interface IDatabase
    {
        IMongoDatabase GetDatabase();
    }

    public class Database : IDatabase
    {
        public readonly DatabaseSettings dbSettings;
        public readonly MongoClient client;
        public readonly IMongoDatabase database;

        public Database(IOptions<DatabaseSettings> options)
        {
            dbSettings = options.Value;
            client = new MongoClient(dbSettings.ConnectionString);
            database = client.GetDatabase(dbSettings.DatabaseName);
        }

        public IMongoDatabase GetDatabase() => database;
    }
}