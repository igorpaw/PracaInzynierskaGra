using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEyetracking;

public class Control : MonoBehaviour {
    public float speed = 300;

    void FixedUpdate()
    {
        MoveLeft();
        MoveRight();
    }
    public void MoveRight()
    {
        // Get Horizontal Input
        float h = Input.GetAxisRaw("Horizontal");

        // Set Velocity (movement direction * speed)
        GetComponent<Rigidbody2D>().velocity = Vector2.right * h * speed;
    }
    public void MoveLeft()
    {
        // Get Horizontal Input
        float h = Input.GetAxisRaw("Horizontal");

        // Set Velocity (movement direction * speed)
        GetComponent<Rigidbody2D>().velocity = Vector2.left * h * speed;
    }
    
    
}
