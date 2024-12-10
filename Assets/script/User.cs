using System;
using MongoDB.Bson;

public class User
{
    public string Name { get; }
    public string PasswordHash { get; }
    public int Aether { get; }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public string WeaponL { get; set; }
    public string WeaponR { get; set; }
    public bool IsOnline { get; private set; }

    public User(string name, string passwordHash, int aether, int posX, int posY, string weaponR, string weaponL, bool isOnline)
    {
        Name = name;
        PasswordHash = passwordHash;
        Aether = aether;
        PosX = posX;
        PosY = posY;
        WeaponL = weaponL;
        WeaponR = weaponR;
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
                    { "posX", PosX },
                    { "posY",  PosY },
                    { "weaponL",  WeaponL },
                    { "weaponR",  WeaponR },
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
