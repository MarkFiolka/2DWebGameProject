using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

public class AuthenticationService
{
    private readonly DatabaseService _dbService;

    public AuthenticationService(DatabaseService dbService)
    {
        _dbService = dbService;
    }

    public DatabaseService GetDatabaseService()
    {
        return _dbService;
    }

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        var playerCollection = _dbService.GetPlayerCollection();
        var user = await FindOrCreatePlayerData.FindPlayerByUsername(playerCollection, username);

        if (user == null)
        {
            Log.Write($"Login failed: User '{username}' not found.");
            return LoginResult.UserNotFound;
        }

        if (user.IsOnline)
        {
            Log.Write($"Login denied: User '{username}' is already online.");
            return LoginResult.UserAlreadyOnline;
        }

        string hashedPassword = PasswordUtility.HashPassword(password);
        if (user.PasswordHash != hashedPassword)
        {
            Log.Write($"Login failed: Incorrect password for user '{username}'.");
            return LoginResult.InvalidCredentials;
        }

        var update = Builders<BsonDocument>.Update.Set("user.isOnline", true);
        var updateResult = await playerCollection.UpdateOneAsync(
            Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("user.name", username),
                Builders<BsonDocument>.Filter.Eq("user.isOnline", false)
            ),
            update
        );

        if (updateResult.ModifiedCount == 0)
        {
            Log.Write($"Login failed: User '{username}' is already online or could not be updated.");
            return LoginResult.UserAlreadyOnline;
        }

        Log.Write($"User '{username}' logged in successfully and is now online.");
        return LoginResult.Success;
    }
}