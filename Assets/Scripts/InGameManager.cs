using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class InGameManager : MonoBehaviour
{
    public GameObject prefab;
    public GameObject bobusPrefab;
    public LevelControler levelConfig;

    public static float timer = 0.0f;
    private static string name;
    public FileStream fs; 
    private SettingsManager settingsManager;
    private int level = 0;
    public Ball ball;
    public Racket racket;
    public static bool bonus;

    public StreamWriter m_WriterParameter;
    void Start()
    {
        ResetTimer();
        settingsManager = ScriptableObject.CreateInstance("SettingsManager") as SettingsManager;
        LevelInit();
        name = Path.GetFullPath(".") + "/" +  System.DateTime.Now.ToString("yyyy_MM_dd") + "_" + settingsManager.sett.steeringMethod + ".csv";
        if (File.Exists(name))
        {
            File.WriteAllText(name, string.Empty);
        }
    }

    public static void ResetTimer()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown("q"))
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    public void BlockInit(int gridY, int gridX, int spacing)
    {
        for (int y = -2; y < gridY; y++)
        {
            for (int x = -4; x < gridX; x++)
            {
                Vector3 pos = new Vector3(x, y, 0) * spacing;
                Instantiate(prefab, pos, Quaternion.identity);
            }
        }
    }

    public void LevelInit()
    {
        LevelConfig conf = levelConfig.listOfLevels[level];
        bonus = conf.bonus;
        Ball.actualLevelNumber = level;
        Ball.bonus = conf.bonus;
        if (level != 0)
        {
            ball.ResetPosition();
            racket.ResetPosition();
        }
        if (!conf.bonus)
        {
            BlockInit(conf.numberOfLines - 2, conf.numberOfColumns,20);
        }
        else
        {
            InitBonusLevel();
        }

        level++;
    }

    public void InitBonusLevel()
    {
        Vector3 pos = new Vector3(UnityEngine.Random.Range(-160.0f, 160.0f), UnityEngine.Random.Range(-80.0f, 80.0f), 0);
        Instantiate(bobusPrefab, pos, Quaternion.identity);
    }

    public static void AddLog(string collisionTarget, Transform collisionTargetPosition, Transform ballPosition, int score, float speed)
    {
        if (!File.Exists(name) || new FileInfo(name).Length == 0)
        {
            using (StreamWriter sw = File.CreateText(name))
            {
                sw.WriteLine("Time;CollisionTarget;targetPositionX;targetPositionY;ballPositionX;ballPositionY;Score;Speed;Bonus");
                sw.WriteLine(timer + ";" + collisionTarget + ";" +
                             collisionTargetPosition.position.x + ";" +
                             collisionTargetPosition.position.y + ";" +
                             ballPosition.position.x + ";" +
                             ballPosition.position.y + ";" +
                             score + ";" + speed + ";" + bonus);
            }	
        }
        using (StreamWriter sw = File.AppendText(name))
        {
            sw.WriteLine(timer + ";" + collisionTarget + ";" +
                         collisionTargetPosition.position.x + ";" +
                         collisionTargetPosition.position.y + ";" +
                         ballPosition.position.x + ";" +
                         ballPosition.position.y + ";" +
                         score + ";" + speed + ";" + bonus);
        }
    }
}
