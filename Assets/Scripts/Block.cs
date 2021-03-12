using UnityEngine;

public class Block : MonoBehaviour
{

    public int level = 2;
    
    private void Start()
    {
        Recolor();
    }

    private void Recolor()
    {
        if (level == 3)
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        else if(level == 2)
            gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        else if(level == 1)
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Ball")
        {
            InGameManager.AddLog("Block",col.transform,
                gameObject.transform,Ball.TotalScore, Ball.Speed); 
            Ball.TotalScore+=Ball.Multiplier;
            Ball.CurrentSceneScore+=Ball.Multiplier;
            level--;
            Recolor();
            if(GameObject.FindGameObjectsWithTag("Block").Length <= 1)
            {
                Ball.RoundEnd = true;
            }
            if(level == 0)
                Destroy(gameObject);
        }
    }
}
