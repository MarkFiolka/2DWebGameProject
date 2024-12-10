using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

public static class FindOrCreatePlayerData
{
    public static async Task<User> FindPlayerByUsername(IMongoCollection<BsonDocument> collection, string username)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("user.name", username);
            var result = await collection.Find(filter).FirstOrDefaultAsync();

            if (result != null)
            {
                Log.Write($"Raw result from database: {result.ToJson()}");

                var userDocument = result["user"].AsBsonDocument;
                return new User(
                    userDocument["name"].AsString,
                    userDocument["passwordHash"].AsString,
                    userDocument["aether"].AsInt32,
                    userDocument["posX"].AsInt32,
                    userDocument["posY"].AsInt32,
                    userDocument["weaponL"].AsString,
                    userDocument["weaponR"].AsString,
                    userDocument.GetValue("isOnline", false).AsBoolean
                );
            }

            Log.Write($"User '{username}' not found in the database.");
            return null;
        }
        catch (Exception ex)
        {
            Log.Write($"Error finding user '{username}': {ex.Message}");
            return null;
        }
    }

    public static async Task CreatePlayer(IMongoCollection<BsonDocument> collection, User newUser)
    {
        var existingPlayer = await FindPlayerByUsername(collection, newUser.Name);
        if (existingPlayer != null)
        {
            Log.Write($"A user with the name '{newUser.Name}' already exists.");
            throw new InvalidOperationException($"A user with the name '{newUser.Name}' already exists.");
        }

        newUser.SetOnlineStatus(false);

        var bsonDocument = newUser.ToBsonDocument();
        await collection.InsertOneAsync(bsonDocument);
        Log.Write($"New user '{newUser.Name}' created successfully and set to offline.");
    }

    public static async Task UpdatePlayerOnlineStatus(IMongoCollection<BsonDocument> collection, string username, bool isOnline)
    {
        try
        {
            Log.Write($"Attempting to update online status for user '{username}' to {isOnline}.");

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
        catch (Exception ex)
        {
            Log.Write($"Error updating online status for user '{username}': {ex.Message}");
        }
    }

    public static async Task UpdatePlayerPos(IMongoCollection<BsonDocument> collection, string username, int posX, int posY)
    {
        try
        {
            Log.Write($"Attempting to update position for user '{username}' to {posX} and position {posY}.");
            
            var filter = Builders<BsonDocument>.Filter.Eq("user.name", username);
            
            var updatePosX = Builders<BsonDocument>.Update.Set("user.posX", posX);
            var updatePosY = Builders<BsonDocument>.Update.Set("user.posY", posY);
            
            var resultForPosX = await collection.UpdateOneAsync(filter, updatePosX);
            var resultForPosY = await collection.UpdateOneAsync(filter, updatePosY);
            
            if (resultForPosX.ModifiedCount > 0 & resultForPosY.ModifiedCount > 0)
            {
                Log.Write($"Attempting to update position for user '{username}' to {posX} and position {posY}.");
            }
            else
            {
                Log.Write($"Failed to update Position of player for user '{username}'.");
            }
        }
        catch (Exception ex)
        {
            Log.Write($"Error updating online status for user '{username}': {ex.Message}");
        }
    }
}