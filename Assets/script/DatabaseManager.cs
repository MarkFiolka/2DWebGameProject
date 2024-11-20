using System;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public void ConnectToDatabase(String username, String password)
    {
        //TODO | CONNECT TO DATABASE AND CHECK USERNAME AND PASSWORD, HASH THE PASSWORD BEFORE CHECKING
        //TODO | IF USERNAME IS SET IN DATABASE FETCH DATA IF NOT CREATE NEW DB PACKAGE USER
        Debug.Log($"Connecting to Database with username: {username} and password: {password}");
    }
}
