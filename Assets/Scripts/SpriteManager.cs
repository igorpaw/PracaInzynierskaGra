using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public enum SpriteEnum
{
    GreenButton,
    OrangeButton
};
public class SpriteManager
{

    public static Sprite GetSprite(SpriteEnum sprite)
    {
        Sprite returnSprite;

        switch (sprite)
        {
           case SpriteEnum.GreenButton:
               returnSprite = GameManager.spriteConfig.greenButton;
               break;
           case SpriteEnum.OrangeButton:
               returnSprite = GameManager.spriteConfig.orangeButton;
               break;
           default:
               returnSprite = GameManager.spriteConfig.greenButton;
               break;
        }

        return returnSprite;
    }
}
