using System;
using TMPro;
using UnityEngine;

public class LoginSceneManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField userNameInputText;
    [SerializeField] private TMP_InputField passwordInputText;
    [SerializeField] private DatabaseManager db;

    private string userNameInput;
    private string passwordInput;

    private void Start()
    {
        if (db == null)
        {
            db = FindObjectOfType<DatabaseManager>();
            if (db == null)
            {
                Debug.LogError("DatabaseManager not found!");
            }
        }
    }

    public void Login()
    {
        if (userNameInputText == null || passwordInputText == null)
        {
            Debug.LogError("Input fields are not assigned in the Inspector!");
            return;
        }

        AssignValues();

        // Validate input
        if (!ValidateInput())
        {
            Debug.LogWarning("Input validation failed. Please correct your input.");
            return;
        }

        TryConnectToDB(userNameInput, passwordInput);
    }

    private void AssignValues()
    {
        userNameInput = userNameInputText.text.Trim();
        passwordInput = passwordInputText.text.Trim();
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(userNameInput) || userNameInput.Length < 3)
        {
            Debug.LogError("Username must have at least 3 characters.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(passwordInput) || passwordInput.Length < 5)
        {
            Debug.LogError("Password must have at least 5 characters.");
            return false;
        }

        return true;
    }

    private void TryConnectToDB(string userName, string password)
    {
        if (db == null)
        {
            Debug.LogError("DatabaseManager is not assigned!");
            return;
        }

        Debug.Log($"Connecting to DB with Username: {userName} and Password: {password}");
        db.ConnectToDatabase(userName, password);
    }
}
