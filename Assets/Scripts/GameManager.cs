using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public SpriteConfig spConfig;
    public static SpriteConfig spriteConfig;

    private void Start()
    {
        spriteConfig = spConfig;
    }
}
