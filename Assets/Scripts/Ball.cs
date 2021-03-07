using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {
    
    public static float Speed;
    private float _timeFromLostLife = 0.0f;
    private float _bonusLevelTime = 0.0f;
    private float _panelTime = 0.0f;
    public static int Multiplier = 1;
    public static int ActualLevelNumber;
    public bool panelInit = false;
    public EndPanel panel;

    private SettingsManager _settingsManager;
    public LevelControler levelConfig;
    public static bool RoundEnd = false;
    
    public static int TotalScore = 0;
    public static int CurrentSceneScore;
    public static int Racket = 5;
    public static int CurrentSceneNum;
    public static bool Bonus = false;

    public InGameManager inGameManager;
    void Start()
    {
        _settingsManager = ScriptableObject.CreateInstance("SettingsManager") as SettingsManager;
        _settingsManager.LoadData();
        panel.Hide();
        if (_settingsManager.sett.opposite == Opposite.Yes)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.down * Speed;
            transform.position = new Vector3(transform.position.x,-transform.position.y,transform.position.z);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * Speed;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Racket" && _settingsManager.sett.opposite == Opposite.No)
        {
            InGameManager.AddLog("Racket",col.transform,gameObject.transform,TotalScore, Speed);
            float x = hitFactor(transform.position,
                col.transform.position,
                col.collider.bounds.size.x);
            
            Vector2 dir = new Vector2(x, 1).normalized;
            
            GetComponent<Rigidbody2D>().velocity =  dir * Speed;
        }
        if (col.gameObject.name == "Racket" && _settingsManager.sett.opposite == Opposite.Yes)
        {
            InGameManager.AddLog("Racket",col.transform,gameObject.transform,TotalScore, Speed);
            float x = hitFactor(transform.position,
                col.transform.position,
                col.collider.bounds.size.x);
            
            Vector2 dir = new Vector2(x, -1).normalized;
            
            GetComponent<Rigidbody2D>().velocity = dir * Speed;
        }
        if (col.gameObject.name == "Ground")
        {
            InGameManager.AddLog("Ground",col.transform,gameObject.transform,TotalScore, Speed);
            if(_settingsManager.sett.opposite == Opposite.No)
                LostLife();
        }
        else if(col.gameObject.name == "borderTop")
        {
            InGameManager.AddLog("Top",col.transform,gameObject.transform,TotalScore, Speed);
            if(_settingsManager.sett.opposite == Opposite.Yes)
                LostLife();
        }
        else if(col.gameObject.name == "borderLeft")
            InGameManager.AddLog("LeftBorder",col.transform,gameObject.transform,TotalScore, Speed);
        else if(col.gameObject.name == "borderRight")
            InGameManager.AddLog("RightBorder",col.transform,gameObject.transform,TotalScore, Speed);
        BallDestroy();
    }
    float hitFactor(Vector2 ballPos, Vector2 racketPos,
        float racketWidth)
    {
        return (ballPos.x - racketPos.x) / racketWidth;
    }

    private void Update()
    {
        _timeFromLostLife += Time.deltaTime;
        _bonusLevelTime += Time.deltaTime;
        if (RoundEnd)
        {
            if (!panelInit)
            {
                panel.Init(CurrentSceneScore, TotalScore, _bonusLevelTime, Bonus);
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) * Speed;
                panelInit = true;
            }
            _panelTime += Time.deltaTime;
            if (_panelTime >= 5.0f)
            {
                panelInit = false;
                NextLevel();
            }
        }
        if (levelConfig.listOfLevels[ActualLevelNumber].bonus)
        {
            if(_bonusLevelTime >= 120.0f)
                NextLevel();
        }
        if (_timeFromLostLife > 30.0f)
        {
            Multiplier++;
            _timeFromLostLife = 0;
        }
    }
    
    public void ResetPosition()
    {
        if (_settingsManager.sett.opposite == Opposite.Yes)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.down * Speed;
            transform.position = new Vector3(0.2f,57.9f,-2.6875f);
        }
        else
        {
            transform.position = new Vector3(0.2f,-57.9f,-2.6875f);
            GetComponent<Rigidbody2D>().velocity = Vector2.up * Speed;
        }
    }

    private void LostLife()
    {
        if(_settingsManager.sett.lostLives == LostLives.Yes)
            Racket--;
        Multiplier = 1;
        _timeFromLostLife = 0.0f;
    }
    
    void BallDestroy()
    {
        if (Racket == 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("LoseScene", LoadSceneMode.Single);
        }
    }

    private void NextLevel()
    {
        panel.Hide();
        RoundEnd = false;
        _panelTime = 0.0f;
        Racket++;
        Speed += (float)10;
        CurrentSceneScore = 0;
        _bonusLevelTime = 0.0f;
        inGameManager.LevelInit();
    }
}
