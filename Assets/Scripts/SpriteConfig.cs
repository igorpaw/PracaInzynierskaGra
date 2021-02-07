using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteConfig", menuName = "Core/Generate Sprite Config", order = 1)]
public class SpriteConfig : ScriptableObject
{
    [SerializeField] public Sprite greenButton;
    [SerializeField] public Sprite orangeButton;
}
