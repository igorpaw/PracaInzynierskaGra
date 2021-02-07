using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartBtnn : MonoBehaviour {

    public void OnBtnClick()
    {
		SceneManager.LoadScene("FirstLevelScene", LoadSceneMode.Single);
        Ball.racket = 5;
		Ball.total_score = 0;
		Ball.current_scene_score = 0;
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
