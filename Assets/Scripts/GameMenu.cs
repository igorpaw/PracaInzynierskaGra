using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private bool _reverse = false;
    public float easy = 80.0f;
    public float medium = 100.0f;
    public float hard = 120.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void ChangeReverse()
    {
        _reverse = !_reverse;
    }
    
    public void Back()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    
    public void OnEasyStartClick()
    {
        Ball.Speed = easy;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    
    public void OnMediumStartClick()
    {
        Ball.Speed = medium;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    
    public void OnHardStartClick()
    {
        Ball.Speed = hard;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
