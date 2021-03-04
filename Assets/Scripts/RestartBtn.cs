using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartBtn : MonoBehaviour {

    public void OnBtnClick()
    {
		SceneManager.LoadScene("FirstLevelScene", LoadSceneMode.Single);
        Ball.Racket = 5;
		Ball.TotalScore = 0;
		Ball.CurrentSceneScore = 0;
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
