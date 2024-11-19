using System;
using UnityEngine;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

public class DBHelloWorld : MonoBehaviour
{
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> collection;

    private String msg = "IT WORKED!";
    
    void Start()
    {
        ConnectToDB();
        //FetchAndPrintMessage();
        //SendToDataBase(msg);
        //GetPlayerData("velw");
        SendComplexToDB("velw", 999);
        UpdatePlayer("velw", 4654564654646);
    }

    private void ConnectToDB()
    {
        string connectionString = "mongodb://localhost:27017";
        client = new MongoClient(connectionString);
        database = client.GetDatabase("GameDB");
        collection = database.GetCollection<BsonDocument>("HelloWorld");
    }
    
    private async void FetchAndPrintMessage()
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Exists("message");
            var result = await collection.Find(filter).FirstOrDefaultAsync();

            if (result != null && result.Contains("message"))
            {
                string message = result["message"].AsString;
                Debug.Log("Message from MongoDB: " + message);
            }
            else
            {
                Debug.Log("No message found in MongoDB.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error fetching message: " + ex.Message);
        }
    }

    private async Task SendToDataBase(string msg)
    {
        try
        {
            Debug.Log("Preparing to send message to MongoDB...");
            var document = new BsonDocument { { "message", msg } };
            await collection.InsertOneAsync(document);
            Debug.Log("Message sent to MongoDB: " + msg);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error sending message to MongoDB: " + ex.Message);
        }
    }

    private async Task GetPlayerData(string userName)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("name", userName);
            var result = await collection.Find(filter).FirstOrDefaultAsync();

            if (result != null)
            {
                string name = result["name"].AsString;
                float aether = result["aether"].IsDouble ? (float)result["aether"].AsDouble : result["aether"].AsInt32;

                Debug.Log($"Player found: Name = {name}, Aether = {aether}");
            }
            else
            {
                Debug.Log($"No player found with the username {userName}.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error fetching player data from MongoDB: " + ex.Message);
        }
    }

    private async Task SendComplexToDB(String name, float aether)
    {
        try
        {
            var document = new BsonDocument { { "name", name }, 
                { "aether", BsonValue.Create((double)aether) } };
            await collection.InsertOneAsync(document);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error sending message to MongoDB: " + ex.Message);
        }
    }

    private async Task UpdatePlayer(string userName, float newAether)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("name", userName);
            var update = Builders<BsonDocument>.Update.Set("aether", newAether);
            var result = await collection.UpdateOneAsync(filter, update);

            if (result.ModifiedCount > 0)
            {
                Debug.Log($"Successfully updated {userName}'s aether to {newAether}.");
            }
            else
            {
                Debug.Log($"No player found with the username {userName}.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error updating player's aether in MongoDB: " + ex.Message);
        }
    }

}
