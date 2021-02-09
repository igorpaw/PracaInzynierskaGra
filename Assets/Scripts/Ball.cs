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
    private float panelTime = 0.0f;
    public static int multiplier = 1;
    public static int actualLevelNumber;
    public EndPanel panel;

    private SettingsManager settingsManager;
    public LevelControler levelConfig;
    private bool roundEnd = false;
    
    public static int total_score = 0;
    public static int current_scene_score;
    public static int racket = 5;
    public static int currentSceneNum;
    public static bool bonus = false;

    public InGameManager inGameManager;
    // Use this for initialization
    void Start()
    {
        settingsManager = ScriptableObject.CreateInstance("SettingsManager") as SettingsManager;
        settingsManager.LoadData();
        panel.Hide();
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
        if (col.gameObject.name == "Racket" && settingsManager.sett.opposite == Opposite.No)
        {
            InGameManager.AddLog("Racket",col.transform,gameObject.transform,total_score, speed);
            float x = hitFactor(transform.position,
                col.transform.position,
                col.collider.bounds.size.x);
            
            Vector2 dir = new Vector2(x, 1).normalized;
            
            GetComponent<Rigidbody2D>().velocity =  dir * speed;
        }
        if (col.gameObject.name == "Racket" && settingsManager.sett.opposite == Opposite.Yes)
        {
            InGameManager.AddLog("Racket",col.transform,gameObject.transform,total_score, speed);
            float x = hitFactor(transform.position,
                col.transform.position,
                col.collider.bounds.size.x);
            
            Vector2 dir = new Vector2(x, -1).normalized;
            
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
        if (roundEnd)
        {
            panelTime += Time.deltaTime;
            if(panelTime >= 5.0f)
                NextLevel();
        }
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
    
    void BallDestroy()
    {
        if (racket == 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("LoseScene", LoadSceneMode.Single);
        }
        
        if(GameObject.FindGameObjectsWithTag("Block").Length < 1)
        {
            roundEnd = true;
            panel.Init(current_scene_score, total_score, bonusLevelTime, bonus);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) * speed;
        }
    }

    private void NextLevel()
    {
        panel.Hide();
        roundEnd = false;
        panelTime = 0.0f;
        racket++;
        speed += (float)10;
        current_scene_score = 0;
        bonusLevelTime = 0.0f;
        inGameManager.LevelInit();
    }
}
