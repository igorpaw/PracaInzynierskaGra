using System;
using System.Collections;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {

    // Movement Speed
    public static float speed;
    private float timeFromLostLife = 0.0f;
    private float bonusLevelTime = 0.0f;
    public static int multiplier = 1;
    public static int actualLevelNumber;

    private SettingsManager settingsManager;
    public LevelControler levelConfig;

    public InGameManager inGameManager;
    // Use this for initialization
    void Start()
    {
        settingsManager = ScriptableObject.CreateInstance("SettingsManager") as SettingsManager;
        settingsManager.LoadData();
        if (settingsManager.sett.opposite == Opposite.Yes)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;
            transform.position = new Vector3(transform.position.x,-transform.position.y,transform.position.z);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        // Hit the Racket?
        if (col.gameObject.name == "Racket" && settingsManager.sett.opposite == Opposite.No)
        {
            InGameManager.AddLog("Racket",col.transform,gameObject.transform,total_score, speed);
            // Calculate hit Factor
            float x = hitFactor(transform.position,
                col.transform.position,
                col.collider.bounds.size.x);

            // Calculate direction, set length to 1
            Vector2 dir = new Vector2(x, 1).normalized;

            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity =  dir * speed;
        }
        if (col.gameObject.name == "Racket" && settingsManager.sett.opposite == Opposite.Yes)
        {
            InGameManager.AddLog("Racket",col.transform,gameObject.transform,total_score, speed);
            // Calculate hit Factor
            float x = hitFactor(transform.position,
                col.transform.position,
                col.collider.bounds.size.x);

            // Calculate direction, set length to 1
            Vector2 dir = new Vector2(x, -1).normalized;

            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }
        if (col.gameObject.name == "Ground")
        {
            InGameManager.AddLog("Ground",col.transform,gameObject.transform,total_score, speed);
            if(settingsManager.sett.opposite == Opposite.No)
                LostLife();
        }
        else if(col.gameObject.name == "borderTop")
        {
            InGameManager.AddLog("Top",col.transform,gameObject.transform,total_score, speed);
            if(settingsManager.sett.opposite == Opposite.Yes)
                LostLife();
        }
        else if(col.gameObject.name == "borderLeft")
            InGameManager.AddLog("LeftBorder",col.transform,gameObject.transform,total_score, speed);
        else if(col.gameObject.name == "borderRight")
            InGameManager.AddLog("RightBorder",col.transform,gameObject.transform,total_score, speed);
        BallDestroy();
    }
    float hitFactor(Vector2 ballPos, Vector2 racketPos,
        float racketWidth)
    {
        return (ballPos.x - racketPos.x) / racketWidth;
    }

    private void Update()
    {
        timeFromLostLife += Time.deltaTime;
        bonusLevelTime += Time.deltaTime;
        if (levelConfig.listOfLevels[actualLevelNumber].bonus)
        {
            if(bonusLevelTime >= 120.0f)
                NextLevel();
        }
        if (timeFromLostLife > 30.0f)
        {
            multiplier++;
            timeFromLostLife = 0;
        }
    }
    
    public void ResetPosition()
    {
        if (settingsManager.sett.opposite == Opposite.Yes)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;
            transform.position = new Vector3(0.2f,57.9f,-2.6875f);
        }
        else
        {
            transform.position = new Vector3(0.2f,-57.9f,-2.6875f);
            GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
        }
    }

    private void LostLife()
    {
        if(settingsManager.sett.lostLives == LostLives.Yes)
            racket--;
        multiplier = 1;
        timeFromLostLife = 0.0f;
    }

    public static int total_score = 0;
    public static int current_scene_score;
    public static int racket = 5;
    public static int currentSceneNum;
    //using UnityEngine.SceneManagement;
    void OnGUI()
    {
        int x = 5;
        int y = 5;
        int h = 100;
        int w = 150;
        int step = 15;
        GUI.Box(new Rect(x, y, w, h), SceneManager.GetActiveScene().name);
		GUI.BeginGroup(new Rect(x, y, w, h), "");
		GUI.Label(new Rect(x + step, y + step, w - 2 * step, 20), "Lifes: " + racket);
        GUI.Label(new Rect(x + step, y + 2 * step, w - 2 * step, 20), "Total score: " + total_score);
		GUI.Label(new Rect(x + step, y + 3 * step, w - 2 * step, 20), "Level score: " + current_scene_score);
		GUI.Label(new Rect(x + step, y + 4 * step, w - 2 * step, 20), "Speed: " + speed/100 + "x");
        GUI.EndGroup();

    }
    void BallDestroy()
    {
        if (racket == 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("LoseScene", LoadSceneMode.Single);
        }
        
        if(GameObject.FindGameObjectsWithTag("Block").Length < 1)
        {
            NextLevel();
        }
    }

    private void NextLevel()
    {
        racket++;
        speed += (float)30;
        current_scene_score = 0;
        bonusLevelTime = 0.0f;
        inGameManager.LevelInit();
    }
}
