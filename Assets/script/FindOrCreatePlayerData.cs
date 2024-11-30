using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

public static class FindOrCreatePlayerData
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

        newUser.SetOnlineStatus(true);

        var bsonDocument = newUser.ToBsonDocument();

        await collection.InsertOneAsync(bsonDocument);
        Log.Write($"New user '{newUser.Name}' created successfully and set to online.");
    }


    public static async Task<bool> IsPlayerOnline(IMongoCollection<BsonDocument> collection, string username)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("user.name", username);
        var player = await collection.Find(filter).FirstOrDefaultAsync();

        if (player != null && player.Contains("user") && player["user"].AsBsonDocument.Contains("isOnline"))
        {
            bool isOnline = player["user"]["isOnline"].AsBoolean;
            Log.Write($"User '{username}' online status is: {isOnline}");
            return isOnline;
        }

        Log.Write($"User '{username}' not found or missing 'isOnline' field.");
        return false;
    }


    public static async Task UpdatePlayerOnlineStatus(IMongoCollection<BsonDocument> collection, string username, bool isOnline)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("user.name", username);
        var update = Builders<BsonDocument>.Update.Set("user.isOnline", isOnline);

        var result = await collection.UpdateOneAsync(filter, update);
        if (result.ModifiedCount > 0)
        {
            Log.Write($"User '{username}' online status updated to {isOnline}.");
        }
        else
        {
            Log.Write($"Failed to update online status for user '{username}'. Check if the user exists.");
        }
    }
}