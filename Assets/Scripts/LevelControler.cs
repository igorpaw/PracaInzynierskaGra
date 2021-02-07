using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsConfig", menuName = "Core/Generate Level config", order = 2)]
public class LevelControler  : ScriptableObject
{
        [SerializeField] public List<LevelConfig> listOfLevels;
}

