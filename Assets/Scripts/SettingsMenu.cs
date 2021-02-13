using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Settings
{
    public class SettingsMenu : MonoBehaviour
    {
        private SettingsManager settingsManager;

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
            settingsManager = ScriptableObject.CreateInstance("SettingsManager") as SettingsManager;
            settingsManager.LoadData();
            SetButtons();
        }
        
        
        public void Back()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        private void SetButtons()
        {
            if(settingsManager.sett.opposite.Equals(Opposite.Yes))
                OppositeButtonClick(oppositeButtonYes,oppositeButtonNo);
            else
                OppositeButtonClick(oppositeButtonNo,oppositeButtonYes);
            if (settingsManager.sett.lostLives.Equals(LostLives.Yes))
                ClickLostYesButton();
            else
                ClickLostNoButton();
            switch (settingsManager.sett.steeringMethod)
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

        private void OppositeButtonClick(GameObject buttonChoosen, params GameObject[] buttonsElse)
        {
            buttonChoosen.GetComponent<SpriteRenderer>().sprite = SpriteManager.GetSprite(SpriteEnum.GreenButton);
            foreach (var button in buttonsElse)
            {
                button.GetComponent<SpriteRenderer>().sprite = SpriteManager.GetSprite(SpriteEnum.OrangeButton);  
            }
        }
        

        public void ClickYesButton()
        {
            settingsManager.sett.opposite = Opposite.Yes;
            settingsManager.SaveData();
            OppositeButtonClick(oppositeButtonYes,oppositeButtonNo);
        }
        
        public void ClickNoButton()
        {
            settingsManager.sett.opposite = Opposite.No;
            settingsManager.SaveData();
            OppositeButtonClick(oppositeButtonNo,oppositeButtonYes);
        }
        
        public void ClickVisibleYesButton()
        {
            settingsManager.sett.showArrows = Visible.Yes;
            settingsManager.SaveData();
            OppositeButtonClick(visibleButtonYes,visibleButtonNo);
        }
        
        public void ClickVisibleNoButton()
        {
            settingsManager.sett.showArrows = Visible.No;
            settingsManager.SaveData();
            OppositeButtonClick(visibleButtonNo,visibleButtonYes);
        }
        
        public void ClickLostYesButton()
        {
            settingsManager.sett.lostLives = LostLives.Yes;
            settingsManager.SaveData();
            OppositeButtonClick(lostButtonYes,lostButtonNo);
        }
        
        public void ClickLostNoButton()
        {
            settingsManager.sett.lostLives = LostLives.No;
            settingsManager.SaveData();
            OppositeButtonClick(lostButtonNo,lostButtonYes);
        }
        
        public void ClickBlinkButton()
        {
            settingsManager.sett.steeringMethod = SteeringMethod.EyesClosure;
            settingsManager.SaveData();
            OppositeButtonClick(blinkButton,moveButton,arrowsButton,gestButton, gestConButton, gestTierButton, arrowsAddButton, moveAddButton);
        }

        public void ClickMoveButton()
        {
            settingsManager.sett.steeringMethod = SteeringMethod.EyesPosition;
            settingsManager.SaveData();
            OppositeButtonClick(moveButton,arrowsButton,blinkButton,gestButton, gestConButton, gestTierButton, arrowsAddButton, moveAddButton);
        }
        
        public void ClickArrowsButton()
        {
            settingsManager.sett.steeringMethod = SteeringMethod.Arrows;
            settingsManager.SaveData();
            OppositeButtonClick(arrowsButton,moveButton,blinkButton,gestButton, gestConButton, gestTierButton, arrowsAddButton, moveAddButton);
        }
        
        public void ClickGestTierButton()
        {
            settingsManager.sett.steeringMethod = SteeringMethod.Tier;
            settingsManager.SaveData();
            OppositeButtonClick(gestTierButton,arrowsButton,moveButton,blinkButton,gestButton, gestConButton, arrowsAddButton, moveAddButton);
        }
        
        public void ClickGestureButton()
        {
            settingsManager.sett.steeringMethod = SteeringMethod.Gesture;
            settingsManager.SaveData();
            OppositeButtonClick(gestButton,arrowsButton,moveButton,blinkButton, gestConButton, gestTierButton, arrowsAddButton, moveAddButton);
        }
        
        public void ClickGestConButton()
        {
            settingsManager.sett.steeringMethod = SteeringMethod.GestCon;
            settingsManager.SaveData();
            OppositeButtonClick( gestConButton,gestTierButton,arrowsButton,moveButton,blinkButton,gestButton, arrowsAddButton, moveAddButton);
        }
        
        public void ClickArrowsAddButton()
        {
            settingsManager.sett.steeringMethod = SteeringMethod.ArrowsAdd;
            settingsManager.SaveData();
            OppositeButtonClick(arrowsAddButton, gestConButton,gestTierButton,arrowsButton,moveButton,blinkButton,gestButton, moveAddButton);
        }
        
        public void ClickMoveAddButton()
        {
            settingsManager.sett.steeringMethod = SteeringMethod.MoveAdd;
            settingsManager.SaveData();
            OppositeButtonClick(moveAddButton, arrowsAddButton, gestConButton,gestTierButton,arrowsButton,moveButton,blinkButton,gestButton);
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

