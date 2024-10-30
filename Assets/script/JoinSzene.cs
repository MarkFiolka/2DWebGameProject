using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinSzene : MonoBehaviour
{
    public void LoadGameSzene()
    {
        LoadScene("GameSzene");
    }

    public void LoadWelcomeSzene()
    {
        LoadScene("WelcomeSzene");
    }
    
    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}