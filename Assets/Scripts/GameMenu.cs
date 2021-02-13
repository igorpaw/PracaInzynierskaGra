using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private bool reverse = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void ChangeReverse()
    {
        reverse = !reverse;
    }
    
    public void Back()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    
    public void OnEasyStartClick()
    {
        Ball.speed = 80.0f;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    
    public void OnMediumStartClick()
    {
        Ball.speed = 100.0f;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    
    public void OnHardStartClick()
    {
        Ball.speed = 120.0f;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
