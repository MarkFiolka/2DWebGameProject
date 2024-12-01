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

        // Validate input
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

                ChangeScene("GameScene");
                InstantiateSzene();
                break;

            case LoginResult.UserNotFound:
                _popupService.ShowTimed("Username not registered. Would you like to create a new account?", 10f);
                bool wantsToRegister = await ConfirmRegistration();

                if (wantsToRegister)
                {
                    await Register(username, password);
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
    }

    private bool _isChangingScene = false;

    public void ChangeScene(string sceneName)
    {
        _isChangingScene = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
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
            onYes: () =>
            {
                taskCompletionSource.SetResult(true);
            },
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
        var newUser = new User(username, hashedPassword, 0, false);

        var playerCollection = _authService.GetDatabaseService().GetPlayerCollection();
        await FindOrCreatePlayerData.CreatePlayer(playerCollection, newUser);

        _popupService.ShowTimed($"Account created successfully for {username}. You can now log in.", 2f);
    }
}