using System;
using TMPro;
using UnityEngine;

public class LoginScene : MonoBehaviour
{
    [SerializeField] private TMP_InputField userNameInputText;
    [SerializeField] private TMP_InputField passwordInputText;
    [SerializeField] private DatabaseConnect dc;
    [SerializeField] private PopupPanel popupPanel;

    private string userNameInput;
    private string passwordInput;

    private void Start()
    {
        if (dc == null)
        {
            dc = FindObjectOfType<DatabaseConnect>();
            if (dc == null)
            {
                Debug.LogError("DatabaseManager not found!");
            }
        }

        if (popupPanel == null)
        {
            popupPanel = FindObjectOfType<PopupPanel>();
            if (popupPanel == null)
            {
                Debug.LogError("PopupPanel not found in the scene!");
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

        if (!ValidateInput())
        {
            Debug.LogWarning("Input validation failed. Please correct your input.");
            return;
        }

        dc?.InitialiseDatabase(userNameInput, passwordInput);
    }

    private void AssignValues()
    {
        userNameInput = userNameInputText.text.Trim();
        passwordInput = passwordInputText.text.Trim();
    }

    private bool ValidateInput()
    {
        if (popupPanel == null)
        {
            Debug.LogError("PopupPanel is not assigned or found. Cannot display popup messages.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(userNameInput) || userNameInput.Length < 3)
        {
            Log.Write("Username must have at least 3 characters.");
            popupPanel.ShowPopup("Username must have at least 3 characters.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(passwordInput) || passwordInput.Length < 5)
        {
            Log.Write("Password must have at least 5 characters.");
            popupPanel.ShowPopup("Password must have at least 5 characters.");
            return false;
        }

        return true;
    }
}