using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

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
