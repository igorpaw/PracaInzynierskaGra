using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameLab.Eyetracking;
using UnityEngine.UI;
using UnityEyetracking;

public class MainMenu : MonoBehaviour
{

    public void OnStartClick()
    {
        SceneManager.LoadScene("GameMenu", LoadSceneMode.Single);
    }
    
    public void OnSettingsClick()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Single);
    }
    

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
