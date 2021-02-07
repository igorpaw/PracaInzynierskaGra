using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEyetracking;


public class Pointer : MonoBehaviour
{
    void Update()
    {
        EyetrackerControll();
    }

    void EyetrackerControll()
    {
        if (UnityEyetracker.et != null)
        {
            Vector3 position = new Vector3();

            position.x = UnityEyetracker.et.AveragedEyeData.PositionF.X;
            position.y = Camera.main.pixelHeight - UnityEyetracker.et.AveragedEyeData.PositionF.Y;
            position.z = 10;
            position = Camera.main.ScreenToWorldPoint(position);
            transform.position = position;
        }
    }
}
