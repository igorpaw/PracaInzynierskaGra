using System.IO;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public GameObject prefab;
    public GameObject bobusPrefab;
    public LevelControler levelConfig;

    private static float _timer = 0.0f;
    private static string name;
    public FileStream fs; 
    private SettingsManager settingsManager;
    private int _level = 0;
    public Ball ball;
    public Racket racket;
    private static bool _bonus;

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
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
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
                Vector3 pos = new Vector3(x * spacing, y * spacing/2, 0);
                Instantiate(prefab, pos, Quaternion.identity);
            }
        }
    }

    public void LevelInit()
    {
        LevelConfig conf = levelConfig.listOfLevels[_level];
        _bonus = conf.bonus;
        Ball.ActualLevelNumber = _level;
        Ball.Bonus = conf.bonus;
        if (_level != 0)
        {
            ball.ResetPosition();
            racket.ResetPosition();
        }
        if (!conf.bonus)
        {
            BlockInit(conf.numberOfLines - 2, conf.numberOfColumns,35);
        }
        else
        {
            InitBonusLevel();
        }

        _level++;
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
                sw.WriteLine(_timer + ";" + collisionTarget + ";" +
                             collisionTargetPosition.position.x + ";" +
                             collisionTargetPosition.position.y + ";" +
                             ballPosition.position.x + ";" +
                             ballPosition.position.y + ";" +
                             score + ";" + speed + ";" + _bonus);
            }	
        }
        using (StreamWriter sw = File.AppendText(name))
        {
            sw.WriteLine(_timer + ";" + collisionTarget + ";" +
                         collisionTargetPosition.position.x + ";" +
                         collisionTargetPosition.position.y + ";" +
                         ballPosition.position.x + ";" +
                         ballPosition.position.y + ";" +
                         score + ";" + speed + ";" + _bonus);
        }
    }
}
