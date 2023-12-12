using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScriptScene : MonoBehaviour
{
    private static string previousScene;

    private void Start()
    {
        LoadNextScene();
    }
    private  void LoadNextScene()
    {

        if (previousScene == "StartScene")
        {
            SceneManager.LoadScene("StoryLine");
        }
        else if (previousScene == "MainScene")
        {
            SceneManager.LoadScene("EndScene");
        }
        else
        {
            Debug.LogWarning("Unknown previous scene.");
        }
    }

    public static void LoadScene(string sceneName)
    {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public static string GetPreviousScene()
    {
        return previousScene;
    }
}
