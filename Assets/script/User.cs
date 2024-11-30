using MongoDB.Bson;

public class User
{
    public string Name { get; }
    public string PasswordHash { get; }
    public int Aether { get; }
    public bool IsOnline { get; private set; }

    public User(string name, string passwordHash, int aether)
    {
        Name = name;
        PasswordHash = passwordHash;
        Aether = aether;
        IsOnline = false;
        
        Log.Write($"Player data is {Name} with Ae {aether}");
    }

    public override string ToString()
    {
        return $"Name: {Name}, Password Hash: {PasswordHash}, Aether: {Aether}";
    }

    public BsonDocument ToBsonDocument()
    {
        return new BsonDocument
        {
            {
                "user", new BsonDocument
                {
                    { "name", Name },
                    { "passwordHash", PasswordHash },
                    { "aether", Aether },
                    { "isOnline", IsOnline }
                }
            }
        };
    }

    public void SetOnlineStatus(bool status)
    {
        IsOnline = status;
    }
}
