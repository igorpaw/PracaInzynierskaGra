using UnityEngine;
using UnityEngine.SceneManagement;

namespace Settings
{
    public class SettingsMenu : MonoBehaviour
    {
        private SettingsManager _settingsManager;

        public GameObject oppositeButtonYes;
        public GameObject oppositeButtonNo;
        
        public GameObject visibleButtonYes;
        public GameObject visibleButtonNo;
        
        public GameObject lostButtonYes;
        public GameObject lostButtonNo;
        
        public GameObject arrowsButton;
        public GameObject moveButton;
        public GameObject blinkButton;
        public GameObject gestButton;
        public GameObject gestTierButton;
        public GameObject gestConButton;
        public GameObject arrowsAddButton;
        public GameObject moveAddButton;
        // Start is called before the first frame update
        void Start()
        {
            _settingsManager = ScriptableObject.CreateInstance("SettingsManager") as SettingsManager;
            _settingsManager.LoadData();
            SetButtons();
        }
        
        
        public void Back()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        private void SetButtons()
        {
            if(_settingsManager.sett.opposite.Equals(Opposite.Yes))
                SettingsButtonClick(oppositeButtonYes,oppositeButtonNo);
            else
                SettingsButtonClick(oppositeButtonNo,oppositeButtonYes);
            if (_settingsManager.sett.lostLives.Equals(LostLives.Yes))
                ClickLostYesButton();
            else
                ClickLostNoButton();
            switch (_settingsManager.sett.steeringMethod)
            {
                case SteeringMethod.GestCon:
                    ClickGestConButton();
                    break;
                case SteeringMethod.Tier:
                    ClickGestTierButton();
                    break;
                case SteeringMethod.Gesture:
                    ClickGestureButton();
                    break;
                case SteeringMethod.ArrowsAdd:
                    ClickArrowsAddButton();
                    break;
                case SteeringMethod.MoveAdd:
                    ClickMoveAddButton();
                    break;
                case SteeringMethod.Arrows:
                    ClickArrowsButton();
                    break;
                case SteeringMethod.EyesClosure:
                    ClickBlinkButton();
                    break;
                case SteeringMethod.EyesPosition:
                    ClickMoveButton();
                    break;
                default:
                    ClickBlinkButton();
                    break;
            }
        }

        private void SettingsButtonClick(GameObject buttonChoosen, params GameObject[] buttonsElse)
        {
            buttonChoosen.GetComponent<SpriteRenderer>().sprite = SpriteManager.GetSprite(SpriteEnum.GreenButton);
            foreach (var button in buttonsElse)
            {
                button.GetComponent<SpriteRenderer>().sprite = SpriteManager.GetSprite(SpriteEnum.OrangeButton);  
            }
        }
        

        public void ClickYesButton()
        {
            _settingsManager.sett.opposite = Opposite.Yes;
            _settingsManager.SaveData();
            SettingsButtonClick(oppositeButtonYes,oppositeButtonNo);
        }
        
        public void ClickNoButton()
        {
            _settingsManager.sett.opposite = Opposite.No;
            _settingsManager.SaveData();
            SettingsButtonClick(oppositeButtonNo,oppositeButtonYes);
        }
        
        public void ClickVisibleYesButton()
        {
            _settingsManager.sett.showArrows = Visible.Yes;
            _settingsManager.SaveData();
            SettingsButtonClick(visibleButtonYes,visibleButtonNo);
        }
        
        public void ClickVisibleNoButton()
        {
            _settingsManager.sett.showArrows = Visible.No;
            _settingsManager.SaveData();
            SettingsButtonClick(visibleButtonNo,visibleButtonYes);
        }
        
        public void ClickLostYesButton()
        {
            _settingsManager.sett.lostLives = LostLives.Yes;
            _settingsManager.SaveData();
            SettingsButtonClick(lostButtonYes,lostButtonNo);
        }
        
        public void ClickLostNoButton()
        {
            _settingsManager.sett.lostLives = LostLives.No;
            _settingsManager.SaveData();
            SettingsButtonClick(lostButtonNo,lostButtonYes);
        }
        
        public void ClickBlinkButton()
        {
            _settingsManager.sett.steeringMethod = SteeringMethod.EyesClosure;
            _settingsManager.SaveData();
            SettingsButtonClick(blinkButton,moveButton,arrowsButton,gestButton, gestConButton, gestTierButton, arrowsAddButton, moveAddButton);
        }

        public void ClickMoveButton()
        {
            _settingsManager.sett.steeringMethod = SteeringMethod.EyesPosition;
            _settingsManager.SaveData();
            SettingsButtonClick(moveButton,arrowsButton,blinkButton,gestButton, gestConButton, gestTierButton, arrowsAddButton, moveAddButton);
        }
        
        public void ClickArrowsButton()
        {
            _settingsManager.sett.steeringMethod = SteeringMethod.Arrows;
            _settingsManager.SaveData();
            SettingsButtonClick(arrowsButton,moveButton,blinkButton,gestButton, gestConButton, gestTierButton, arrowsAddButton, moveAddButton);
        }
        
        public void ClickGestTierButton()
        {
            _settingsManager.sett.steeringMethod = SteeringMethod.Tier;
            _settingsManager.SaveData();
            SettingsButtonClick(gestTierButton,arrowsButton,moveButton,blinkButton,gestButton, gestConButton, arrowsAddButton, moveAddButton);
        }
        
        public void ClickGestureButton()
        {
            _settingsManager.sett.steeringMethod = SteeringMethod.Gesture;
            _settingsManager.SaveData();
            SettingsButtonClick(gestButton,arrowsButton,moveButton,blinkButton, gestConButton, gestTierButton, arrowsAddButton, moveAddButton);
        }
        
        public void ClickGestConButton()
        {
            _settingsManager.sett.steeringMethod = SteeringMethod.GestCon;
            _settingsManager.SaveData();
            SettingsButtonClick( gestConButton,gestTierButton,arrowsButton,moveButton,blinkButton,gestButton, arrowsAddButton, moveAddButton);
        }
        
        public void ClickArrowsAddButton()
        {
            _settingsManager.sett.steeringMethod = SteeringMethod.ArrowsAdd;
            _settingsManager.SaveData();
            SettingsButtonClick(arrowsAddButton, gestConButton,gestTierButton,arrowsButton,moveButton,blinkButton,gestButton, moveAddButton);
        }
        
        public void ClickMoveAddButton()
        {
            _settingsManager.sett.steeringMethod = SteeringMethod.MoveAdd;
            _settingsManager.SaveData();
            SettingsButtonClick(moveAddButton, arrowsAddButton, gestConButton,gestTierButton,arrowsButton,moveButton,blinkButton,gestButton);
        }
        
        
        
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            }
        }
    }
}

