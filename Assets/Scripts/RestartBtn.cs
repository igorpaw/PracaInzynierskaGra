using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartBtn : MonoBehaviour
{
    public Text score;

    private void Start()
    {
        score.text = "Total score: " + Ball.TotalScore;
    }

    public void OnBtnClick()
    {
        Ball.Speed = 80;
        Ball.Racket = 5;
        Ball.TotalScore = 0;
        Ball.CurrentSceneScore = 0;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void OnQuitClick()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
