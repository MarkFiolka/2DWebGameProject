using System;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class LoginScene : MonoBehaviour
{
    [SerializeField] private TMP_InputField userNameInputText;
    [SerializeField] private TMP_InputField passwordInputText;
    [SerializeField] private PopupPanel popupPanel;

    private AuthenticationService _authService;
    private PopupService _popupService;
    private string _loggedInUsername;
    private GameSzene _gameSzene;
    private User _user;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        if (popupPanel == null)
        {
            Debug.LogError("PopupPanel is not assigned in the Inspector!");
            return;
        }

        DontDestroyOnLoad(gameObject);

        var databaseService = new DatabaseService();
        databaseService.Initialize();

        _authService = new AuthenticationService(databaseService);
        _popupService = new PopupService(popupPanel);
    }

    public async void Login()
    {
        string username = userNameInputText.text.Trim();
        string password = passwordInputText.text.Trim();

        if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
        {
            _popupService.ShowTimed("Username must have at least 3 characters.", 2f);
            return;
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 5)
        {
            _popupService.ShowTimed("Password must have at least 5 characters.", 2f);
            return;
        }

        var loginResult = await _authService.LoginAsync(username, password);

        switch (loginResult)
        {
            case LoginResult.Success:
                _loggedInUsername = username;
                _popupService.ShowTimed("Login successful!", 2f);

                var databaseService = _authService.GetDatabaseService();
                var playerCollection = databaseService.GetPlayerCollection();
                var user = await FindOrCreatePlayerData.FindPlayerByUsername(playerCollection, username);

                if (user == null)
                {
                    _popupService.ShowTimed("Error: User data not found.", 2f);
                    return;
                }

                _user = user; // Cache the user object for later use
                await ChangeSceneAsync("GameScene");

                _gameSzene = FindObjectOfType<GameSzene>();
                if (_gameSzene == null)
                {
                    Debug.LogError("GameSzene could not be found in the scene!");
                    return;
                }
                _gameSzene.SetUser(user);
                _gameSzene.InstantiateSzene();
                
                _playerMovement = FindObjectOfType<PlayerMovement>();
                if (_playerMovement == null)
                {
                    Debug.LogError("PlayerMovement could not be found in the scene!");
                    return;
                }
                _playerMovement.SetUser(user);
                
                break;

            case LoginResult.UserNotFound:
                _popupService.ShowTimed("Username not registered. Would you like to create a new account?", 10f);
                bool wantsToRegister = await ConfirmRegistration();

                if (wantsToRegister)
                {
                    await Register(username, password);
                    Login(); // Retry login after successful registration
                }

                break;

            case LoginResult.InvalidCredentials:
                _popupService.ShowTimed("Invalid username or password.", 2f);
                break;

            case LoginResult.UserAlreadyOnline:
                _popupService.ShowTimed("This account is already logged in. Please try again later.", 3f);
                break;
        }
    }

    private bool _isExiting;

    private void OnApplicationQuit()
    {
        if (_isExiting) return;

        _isExiting = true;

        if (!string.IsNullOrEmpty(_loggedInUsername))
        {
            Task.Run(async () => await FindOrCreatePlayerData.UpdatePlayerOnlineStatus(
                _authService.GetDatabaseService().GetPlayerCollection(),
                _loggedInUsername,
                false)).Wait();
        }

        if (!string.IsNullOrEmpty(_loggedInUsername))
        {
            Task.Run(async () => await FindOrCreatePlayerData.UpdatePlayerPos(
                _authService.GetDatabaseService().GetPlayerCollection(),
                _loggedInUsername,
                _user.PosX,
                _user.PosY)).Wait();
        }
    }

    private bool _isChangingScene = false;

    public async Task ChangeSceneAsync(string sceneName)
    {
        if (_isChangingScene)
            return;

        _isChangingScene = true;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            await Task.Yield();
        }

        _isChangingScene = false;
    }

    private async void OnDestroy()
    {
        if (_isChangingScene) return;

        if (!string.IsNullOrEmpty(_loggedInUsername))
        {
            await FindOrCreatePlayerData.UpdatePlayerOnlineStatus(
                _authService.GetDatabaseService().GetPlayerCollection(),
                _loggedInUsername,
                false
            );
            Log.Write($"User '{_loggedInUsername}' set to offline during OnDestroy.");
        }

        if (!string.IsNullOrEmpty(_loggedInUsername))
        {
            Task.Run(async () => await FindOrCreatePlayerData.UpdatePlayerPos(
                _authService.GetDatabaseService().GetPlayerCollection(),
                _loggedInUsername,
                _user.PosX,
                _user.PosY)).Wait();
        }
    }

    private async Task Logout(string username)
    {
        var playerCollection = _authService.GetDatabaseService().GetPlayerCollection();
        await FindOrCreatePlayerData.UpdatePlayerOnlineStatus(playerCollection, username, false);
        Log.Write($"User '{username}' logged out and set to offline.");
    }

    private Task<bool> ConfirmRegistration()
    {
        var taskCompletionSource = new TaskCompletionSource<bool>();

        _popupService.ShowWithButtons(
            "Would you like to register this account?",
            onYes: () => { taskCompletionSource.SetResult(true); },
            onNo: () =>
            {
                _popupService.ShowTimed("You chose not to register.", 2f);
                taskCompletionSource.SetResult(false);
            },
            timeout: 10f
        );

        return taskCompletionSource.Task;
    }


    private async Task Register(string username, string password)
    {
        var hashedPassword = PasswordUtility.HashPassword(password);
        var newUser = new User(username, 
            hashedPassword, 
            0, 
            0, 
            0, 
            "Rifle", 
            "Rifle",
            false);

        var playerCollection = _authService.GetDatabaseService().GetPlayerCollection();
        try
        {
            await FindOrCreatePlayerData.CreatePlayer(playerCollection, newUser);
            _popupService.ShowTimed($"Account created successfully for {username}. You can now log in.", 2f);
        }
        catch (Exception ex)
        {
            _popupService.ShowTimed($"Failed to create account: {ex.Message}", 2f);
            Log.Write($"Error creating user '{username}': {ex.Message}");
        }
    }
}