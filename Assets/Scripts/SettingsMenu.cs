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
        
        public GameObject keyboardButton;
        public GameObject arrowsButton;
        public GameObject moveButton;
        public GameObject blinkButton;
        public GameObject gestButton;
        
        
        public GameObject whiteButton;
        public GameObject yellowButton;
        public GameObject greenButton;
        public GameObject redButton;
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
            switch (settingsManager.sett.steeringMethod)
            {
                case SteeringMethod.Keyboard:
                    ClickKeyboardButton();
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
                    ClickKeyboardButton();
                    break;
            }

            switch (settingsManager.sett.steeringArrowsColor)
            {
                case SteeringArrowsColor.White:
                    ClickWhiteButton();
                    break;
                case SteeringArrowsColor.Yellow:
                    ClickYellowButton();
                    break;
                case SteeringArrowsColor.Green:
                    ClickGreenButton();
                    break;
                case SteeringArrowsColor.Red:
                    ClickRedButton();
                    break;
                default:
                    ClickWhiteButton();
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
            OppositeButtonClick(blinkButton,keyboardButton,moveButton,arrowsButton,gestButton);
        }

        public void ClickMoveButton()
        {
            settingsManager.sett.steeringMethod = SteeringMethod.EyesPosition;
            settingsManager.SaveData();
            OppositeButtonClick(moveButton,keyboardButton,arrowsButton,blinkButton,gestButton);
        }
        
        public void ClickArrowsButton()
        {
            settingsManager.sett.steeringMethod = SteeringMethod.Arrows;
            settingsManager.SaveData();
            OppositeButtonClick(arrowsButton,keyboardButton,moveButton,blinkButton,gestButton);
        }
        
        public void ClickKeyboardButton()
        {
            settingsManager.sett.steeringMethod = SteeringMethod.Keyboard;
            settingsManager.SaveData();
            OppositeButtonClick(keyboardButton,arrowsButton,moveButton,blinkButton,gestButton);
        }
        
        public void ClickGestureButton()
        {
            settingsManager.sett.steeringMethod = SteeringMethod.Gesture;
            settingsManager.SaveData();
            OppositeButtonClick(gestButton,keyboardButton,arrowsButton,moveButton,blinkButton);
        }
        
        public void ClickWhiteButton()
        {
            settingsManager.sett.steeringArrowsColor = SteeringArrowsColor.White;
            settingsManager.SaveData();
            OppositeButtonClick(whiteButton,yellowButton,greenButton,redButton);
        }

        public void ClickYellowButton()
        {
            settingsManager.sett.steeringArrowsColor = SteeringArrowsColor.Yellow;
            settingsManager.SaveData();
            OppositeButtonClick(yellowButton,whiteButton,greenButton,redButton);
        }
        
        public void ClickGreenButton()
        {
            settingsManager.sett.steeringArrowsColor = SteeringArrowsColor.Green;
            settingsManager.SaveData();
            OppositeButtonClick(greenButton,whiteButton,yellowButton,redButton);
        }
        
        public void ClickRedButton()
        {
            settingsManager.sett.steeringArrowsColor = SteeringArrowsColor.Red;
            settingsManager.SaveData();
            OppositeButtonClick(redButton,whiteButton,yellowButton,greenButton);
        }
        
        void Update()
        {
        
        }
    }
}

