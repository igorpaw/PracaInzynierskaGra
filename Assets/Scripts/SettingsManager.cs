using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Settings
{
    public class SettingsManager : ScriptableObject
    {
        public Settings sett = new Settings();

        public SettingsManager()
        {
            LoadData();
        }
    
        public void SaveData()
        {
            if (!Directory.Exists("Settings"))
                Directory.CreateDirectory("Settings");

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Create("Settings/settings.binary");

            formatter.Serialize(fileStream, sett);

            fileStream.Close();
        }

        public void LoadData()
        {
            if (File.Exists("Settings/settings.binary"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fileStream = File.Open("Settings/settings.binary", FileMode.Open);

                sett = (Settings)formatter.Deserialize(fileStream);

                fileStream.Close();
            }
            else
            {
                ResetData();
                SaveData();
            }
        }
    
        public void ResetData()
        {
            Reset();
        }

        public void Reset()
        {
            sett.steeringMethod = SteeringMethod.Keyboard;
            sett.showArrows = Visible.Yes;
            sett.steeringArrowsColor = SteeringArrowsColor.White;
            sett.opposite = Opposite.No;
            sett.lostLives = LostLives.Yes;

        }
    }
}

