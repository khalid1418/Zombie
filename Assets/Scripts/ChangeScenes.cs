using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene("MainScene");
        
    }
    public void IntroLineScene()
    {
        LoadingScriptScene.LoadScene("LoadingScene");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
