using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

public static class PlayerDatabase
{
    public static async Task<User> FindPlayerByUsername(IMongoCollection<BsonDocument> collection, string username)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("user.name", username);
        var result = await collection.Find(filter).FirstOrDefaultAsync();

        if (result != null)
        {
            Log.Write($"User '{username}' found in the database.");
            var userDocument = result["user"].AsBsonDocument;
            return new User(
                userDocument["name"].AsString,
                userDocument["passwordHash"].AsString,
                userDocument["aether"].AsInt32
            );
        }

        Log.Write($"User '{username}' not found in the database.");
        return null;
    }


    public static async Task CreatePlayer(IMongoCollection<BsonDocument> collection, User newUser)
    {
        var existingPlayer = await FindPlayerByUsername(collection, newUser.Name);
        if (existingPlayer != null)
        {
            Log.Write($"A user with the name '{newUser.Name}' already exists.");
            throw new InvalidOperationException($"A user with the name '{newUser.Name}' already exists.");
        }

        var bsonDocument = newUser.ToBsonDocument();
        await collection.InsertOneAsync(bsonDocument);
        Log.Write($"New user '{newUser.Name}' created successfully.");
    }

}