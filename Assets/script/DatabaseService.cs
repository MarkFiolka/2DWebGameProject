using MongoDB.Bson;
using MongoDB.Driver;

public class DatabaseService
{
    private MongoClient _client;
    private IMongoDatabase _database;
    private IMongoCollection<BsonDocument> _playerCollection;

    public void Initialize()
    {
        _client = new MongoClient("mongodb://localhost:27017");
        _database = _client.GetDatabase("GameDB");
        _playerCollection = _database.GetCollection<BsonDocument>("Players");

        Log.Write("Database initialized successfully.");
    }

    public IMongoCollection<BsonDocument> GetPlayerCollection() => _playerCollection;
}