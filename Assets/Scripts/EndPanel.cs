using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    public Text desc;
    public Text levelScore;
    public Text totalScore;

    public void Init(int levelScore, int totalScore, float time, bool bonus)
    {
        if (bonus)
        {
            desc.text = "Bonus Level Time:";
            this.levelScore.text = (int)time + "s";
        }
        else
        {
            desc.text = "Level Score:";
            this.levelScore.text = levelScore.ToString();
        }
        this.totalScore.text = totalScore.ToString();
        gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
