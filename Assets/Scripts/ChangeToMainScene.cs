using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToMainScene : MonoBehaviour
{
    
    void Start()
    {
        SceneManager.LoadScene("MainScene");
    }

}
