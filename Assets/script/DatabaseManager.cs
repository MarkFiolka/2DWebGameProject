using System;
using UnityEngine;
using MongoDB.Bson;
using MongoDB.Driver;

public class DatabaseManager : MonoBehaviour
{
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> playerCollection;

    public PopupPanel popupPanel;

    public void InitialiseDatabase(string username, string password)
    {
        Debug.Log($"Initialising Database for username: {username}");
        ConnectToDatabase();
        HandlePlayerLoginOrCreation(username, password);
    }

    private void ConnectToDatabase()
    {
        try
        {
            Log.Write("Connecting to Database...");

            client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("GameDB");
            playerCollection = database.GetCollection<BsonDocument>("Players");

            Log.Write("Connected to Database successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error connecting to the database: {ex.Message}");
        }
    }

    private async void HandlePlayerLoginOrCreation(string username, string password)
    {
        try
        {
            if (popupPanel == null)
            {
                Debug.LogError("PopupManager is not assigned.");
                return;
            }

            string hashedPassword = PasswordUtility.HashPassword(password);

            if (playerCollection == null)
            {
                Debug.LogError("playerCollection is null. Database connection might have failed.");
                popupPanel.ShowPopup("Database error. Please try again later.");
                return;
            }
            
            var existingPlayer = await PlayerDatabase.FindPlayerByUsername(playerCollection, username);

            if (existingPlayer != null)
            {
                if (existingPlayer.PasswordHash == hashedPassword)
                {
                    Log.Write($"User '{existingPlayer.Name}' logged in successfully.");
                    Log.Write($"Welcome back, {existingPlayer.Name}!");
                }
                else
                {
                    popupPanel.ShowPopup("Incorrect username or password. Please try again.");
                }
            }
            else
            {
                Log.Write($"No user found with username '{username}'. Creating a new user...");
                var newPlayer = new User(username, hashedPassword, 0);
                await PlayerDatabase.CreatePlayer(playerCollection, newPlayer);

                //continue logic game like close Szene open GameSzene etc
                popupPanel.ShowPopup($"New account created successfully for {username}.");
                Log.Write($"New user created: {newPlayer}");
            }
        }
        catch (InvalidOperationException ex)
        {
            Log.Write(ex.Message);
            popupPanel.ShowPopup(ex.Message);
        }
        catch (Exception ex)
        {
            Log.Write($"Error during player login or creation: {ex.Message}");
            popupPanel.ShowPopup("An unexpected error occurred. Please try again.");
        }
    }

}
