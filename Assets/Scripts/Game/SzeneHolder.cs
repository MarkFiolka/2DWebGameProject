using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SzeneHolder : MonoBehaviour
{
    public void LoadSceneGameScene()
    {
        LoadScene("GameScene");
    }
    
    public void UnloadSceneGameScene()
    {
        UnloadScene("GameScene");
    }
    
    public void LoadSceneWelcomeScene()
    {
        LoadScene("WelcomeScene");
    }
    
    public void UnloadSceneWelcomeScene()
    {
        UnloadScene("WelcomeScene");
    }
    
    public void LoadSceneLoginScene()
    {
        LoadScene("LoginScene");
    }
    
    public void UnloadSceneLoginScene()
    {
        UnloadScene("LoginScene");
    }
    
    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    
    private void UnloadScene(string sceneName)
    {
        StartCoroutine(UnloadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
    private IEnumerator UnloadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}