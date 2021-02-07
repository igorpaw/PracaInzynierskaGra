using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEyetracking;
using Settings;

public class Racket : MonoBehaviour {

    // Movement Speed
    public float speed = 150;
    public Pointer pointer;
    public GameObject rightArrow;
    public GameObject leftArrow;
    private SettingsManager settingsManager;
    private Transform lastTransform;
    private bool lastToLeft = false;
    private bool lastToRight = false;
    private bool lastToLeftStop= false;
    private bool lastToRightStop = false;
    private bool lastUp = false;
    private bool lastDown = false;
    private bool startToRight = false;
    private bool startToLeft = false;
    private bool startToRightStop = false;
    private bool startToLeftStop = false;
    private int tier = 1;
    private Vector3 target;
    private bool steeringGesture = false;
    private bool gestureMove = false;
    public int leftRight = 170;
    public int bottom = 60;
    public int activation = 40;
    
    public int moveSize = 20;
    public float racketLengh = 100f;

    private void Start()
    {
        lastTransform = pointer.transform;
        settingsManager = ScriptableObject.CreateInstance("SettingsManager") as SettingsManager;
        settingsManager.LoadData();
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
        if (settingsManager.sett.opposite.Equals(Opposite.Yes))
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,-gameObject.transform.position.y,gameObject.transform.position.z);
        if (settingsManager.sett.steeringMethod.Equals(SteeringMethod.Arrows))
        {
            InstantiateArrows();
        }
        SpaceShipSteering();
    }


    void FixedUpdate()
    {
        SpaceShipSteering();
        if (steeringGesture && gestureMove)
        {
            var dir=(target - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
            if (target.x >= transform.position.x - 1 && target.x <= transform.position.x + 1)
                gestureMove = false;
        }
    }
    
    private void SpaceShipSteering()
    {
        switch (settingsManager.sett.steeringMethod)
        {
            case SteeringMethod.Keyboard:
                SteeringByKeyboard();
                break;
            case SteeringMethod.Arrows:
                //SteeringByArrows();
                SteeringByArrowsAdaptive();
                break;
            case SteeringMethod.Gesture:
                //SteeringByGesture(true);
                SteeringByGestureConstant();
                StopGesture();
                steeringGesture = true;
                break;
            case SteeringMethod.EyesClosure:
                SteeringByEyesClosure();
                break;
            case SteeringMethod.EyesPosition:
                SteeringByEyesSimple();
                //SteeringByEyesPositionAdaptive();
                break;
            default:
                SteeringByKeyboard();
                break;
        }
    }
    
    private void SteeringByKeyboard()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalMove, 0) * speed;
    }
    
    private void SteeringByEyesClosure()
    {
        if (UnityEyetracker.et != null)
        {
            if (!UnityEyetracker.et.LeftEyeDetected && UnityEyetracker.et.RightEyeDetected)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * speed;
            }
            else if (!UnityEyetracker.et.RightEyeDetected && UnityEyetracker.et.LeftEyeDetected)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * speed;
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) * speed;
            }
        }
    }

    private void StopGesture()
    {
        if ((pointer.transform.position.x > -leftRight && pointer.transform.position.x < leftRight) && pointer.transform.position.y < bottom)
        {
            SetAllStopBool(false);
            lastTransform = pointer.transform;
            return;
        }
        
        if (pointer.transform.position.x >= 0 && pointer.transform.position.x < 40)
        {
            SetAllStopBool(false);
            startToRightStop = true;
            lastTransform = pointer.transform;
            return;
        }

        if (pointer.transform.position.x < 0 && pointer.transform.position.x > -40)
        {
            SetAllStopBool(false);
            startToLeftStop = true;
            lastTransform = pointer.transform;
            return;
        }
        
        if (startToRightStop)
        {
            MoveToRightStop();
        }

        if (startToLeftStop)
        {
            MoveToLeftStop();
        }
    }

    private void SteeringByGestureConstant()
    {
        if ((pointer.transform.position.x > -leftRight && pointer.transform.position.x < leftRight) &&
            pointer.transform.position.y > -bottom)
        {
            SetAllBool(false);
            lastTransform = pointer.transform;
            return;
        }
        if (pointer.transform.position.x >= 0 && pointer.transform.position.x < 40)
        {
            SetAllBool(false);
            startToRight = true;
            lastTransform = pointer.transform;
            return;
        }

        if (pointer.transform.position.x < 0 && pointer.transform.position.x > -40)
        {
            SetAllBool(false);
            startToLeft = true;
            lastTransform = pointer.transform;
            return;
        }
            
        if (startToRight)
        {
            MoveToRight(true);
        }

        if (startToLeft)
        {
            MoveToLeft(true);
        }
            
    }
    

    private void SteeringByGesture(bool activationTiers)
    {
        if ((pointer.transform.position.x > -leftRight && pointer.transform.position.x < leftRight) && pointer.transform.position.y > -bottom)
        {
            SetAllBool(false);
            lastTransform = pointer.transform;
            return;
        }

        if (!activationTiers)
        {
            if (pointer.transform.position.x >= 0 && pointer.transform.position.x < 40)
            {
                SetAllBool(false);
                startToRight = true;
                lastTransform = pointer.transform;
                return;
            }

            if (pointer.transform.position.x < 0 && pointer.transform.position.x > -40)
            {
                SetAllBool(false);
                startToLeft = true;
                lastTransform = pointer.transform;
                return;
            }
        }

        if (activationTiers && !startToLeft && !startToRight)
        {
            if (pointer.transform.position.x >= 0 && pointer.transform.position.x < 30)
            {
                SetTierRight(5);
                return;
            }
            if (pointer.transform.position.x >= 30 && pointer.transform.position.x < 60)
            {
                SetTierRight(4);
                return;
            }
            if (pointer.transform.position.x >= 60 && pointer.transform.position.x < 90)
            {
                SetTierRight(3);
                return;
            }
            if (pointer.transform.position.x >= 90 && pointer.transform.position.x < 120)
            {
                SetTierRight(2);
                return;
            }
            if (pointer.transform.position.x >= 120 && pointer.transform.position.x < 150)
            {
                SetTierRight(1);
                return;
            }
            if (pointer.transform.position.x < 0 && pointer.transform.position.x > -30)
            {
                SetTierLeft(5);
                return;
            }
            if (pointer.transform.position.x < -30 && pointer.transform.position.x > -60)
            {
                SetTierLeft(4);
                return;
            }
            if (pointer.transform.position.x < -60 && pointer.transform.position.x > -90)
            {
                SetTierLeft(3);
                return;
            }
            if (pointer.transform.position.x < -90 && pointer.transform.position.x > -120)
            {
                SetTierLeft(2);
                return;
            }
            if (pointer.transform.position.x < -120 && pointer.transform.position.x > -150)
            {
                SetTierLeft(1);
                return;
            }
        }
        

        if (startToRight)
        {
            MoveToRight(false);
        }

        if (startToLeft)
        {
            MoveToLeft(false);
        }
    }

    private void SetTierRight(int tier)
    {
        SetAllBool(false);
        startToRight = true;
        lastTransform = pointer.transform;
        this.tier = tier;
    }
    
    private void SetTierLeft(int tier)
    {
        SetAllBool(false);
        startToLeft = true;
        lastTransform = pointer.transform;
        this.tier = tier;
    }
    
    private void MoveToRightStop()
    {
        if (pointer.transform.position.x < lastTransform.position.x && pointer.transform.position.x < leftRight)
        {
            SetAllStopBool(false);
            lastTransform = pointer.transform;
            return;
        }

        if (pointer.transform.position.x >= lastTransform.position.x && pointer.transform.position.x < leftRight)
        {
            lastToRightStop = true;
            lastTransform = pointer.transform;
            return;
        }

        if (pointer.transform.position.x > leftRight && lastToRightStop)
        {
            MoveDown();
        }
        
    }

    private void MoveToRight(bool constant)
    {
            if (pointer.transform.position.x < lastTransform.position.x && pointer.transform.position.x < leftRight)
            {
                SetAllBool(false);
                lastTransform = pointer.transform;
                return;
            }

            if (pointer.transform.position.x >= lastTransform.position.x && pointer.transform.position.x < leftRight)
            {
                lastToRight = true;
                lastTransform = pointer.transform;
                return;
            }

            if (pointer.transform.position.x > leftRight && lastToRight)
            {
                MoveUp(true, constant);
            }
        
    }
    
    private void MoveToLeftStop()
    {
        
        if (pointer.transform.position.x > lastTransform.position.x && pointer.transform.position.x > -leftRight)
        {
            SetAllStopBool(false);
            lastTransform = pointer.transform;
            return;
        }

        if (pointer.transform.position.x <= lastTransform.position.x && pointer.transform.position.x > -leftRight)
        {
            lastToLeftStop = true;
            lastTransform = pointer.transform;
            return;
        }

        if (pointer.transform.position.x < -leftRight && lastToLeftStop)
        {
            MoveDown();
        }
        
    }
    
    private void MoveToLeft(bool constant)
    {
        
        if (pointer.transform.position.x > lastTransform.position.x && pointer.transform.position.x > -leftRight)
        {
            SetAllBool(false);
            lastTransform = pointer.transform;
            return;
        }

        if (pointer.transform.position.x <= lastTransform.position.x && pointer.transform.position.x > -leftRight)
        {
            lastToLeft = true;
            lastTransform = pointer.transform;
            return;
        }

        if (pointer.transform.position.x < -leftRight && lastToLeft)
        {
            MoveUp(false, constant);
        }
        
    }

    private void MoveDown()
    {
        if (pointer.transform.position.y > -activation)
        {
            if (lastDown)
            {
                if (pointer.transform.position.y <= lastTransform.position.y)
                {
                    lastDown = true;
                    lastTransform = pointer.transform;
                    return;
                }
                SetAllStopBool(false);
                lastTransform= pointer.transform;
                return;
            }

            lastDown = true;
            lastTransform = pointer.transform;
            return;
        }

        if (lastDown)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) * speed;
            SetAllStopBool(false);
            lastTransform = pointer.transform;
            return;
        }
        SetAllStopBool(false);
        lastTransform = pointer.transform;
    }
    private void MoveUp(bool toRight, bool constant)
    {
        if (pointer.transform.position.y < activation)
        {
            if (lastUp)
            {
                if (pointer.transform.position.y >= lastTransform.position.y)
                {
                    lastUp = true;
                    lastTransform = pointer.transform;
                    return;
                }
                SetAllBool(false);
                lastTransform= pointer.transform;
                return;
            }

            lastUp = true;
            lastTransform = pointer.transform;
            return;
        }

        if (lastUp)
        {
            if (constant)
            {
                if(toRight)
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * speed;
                else
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * speed;
                }
            }
            else
            {
                if (toRight)
                {
                    target = new Vector3(transform.position.x + moveSize*tier, transform.position.y,
                        transform.position.z);
                    gestureMove = true;
                }
                   
                else
                {
                    target = new Vector3(transform.position.x - moveSize*tier, transform.position.y,
                        transform.position.z);
                    gestureMove = true;
                }
            }
            
            SetAllBool(false);
            lastTransform = pointer.transform;
            return;
        }
        SetAllBool(false);
        lastTransform = pointer.transform;
    }

    private void SetAllBool(bool state)
    {
        lastUp = state;
        lastToLeft = state;
        lastToRight = state;
        startToLeft = state;
        startToRight = state;
    }
    
    private void SetAllStopBool(bool state)
    {
        lastDown = state;
        startToLeftStop = state;
        startToRightStop = state;
        startToLeftStop = state;
        startToRightStop = state;
    }
    
    private void SteeringByEyesPositionAdaptive()
    {
        if (UnityEyetracker.et != null)
        {
            if (pointer.transform.position.x < gameObject.transform.position.x)
            {
                float velocity = GetLeftVelocity();
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-velocity, 0) * speed;
            }
            else if (pointer.transform.position.x > gameObject.transform.position.x)
            {
                float velocity = GetRightVelocity();
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, 0) * speed;
            }

            if (Math.Abs(pointer.transform.position.x - gameObject.transform.position.x) < racketLengh / 5)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) * speed;
            }
        }
    }
    
    private float GetLeftVelocity()
    {
        float cursorTransformX = pointer.transform.position.x;
        float spaceShipTransformX = this.gameObject.transform.position.x;
        float distance = cursorTransformX - spaceShipTransformX;

        if (distance < -120)
            return 1f;
        else if (distance > 20)
            return 0.25f;
        else
            return -distance / 120;
    }
    
    private float GetLeftVelocitySide()
    {
        float distance = pointer.transform.position.x;
        
        

        if (distance < -120)
            return 1f;
        else if (distance > 20)
            return 0.25f;
        else
            return -distance / 120;
    }
    
    private float GetRightVelocitySide()
    {
        float distance = pointer.transform.position.x;
        

        if (distance > 120)
            return 1f;
        else if (distance < 20)
            return 0.25f;
        else
            return distance / 120;
    }

    private float GetRightVelocity()
    {
        float cursorTransformX = pointer.transform.position.x;
        float spaceShipTransformX = this.gameObject.transform.position.x;
        float distance = cursorTransformX - spaceShipTransformX;

        if (distance > 120)
            return 1f;
        else if (distance < 20)
            return 0.25f;
        else
            return distance / 120;
    }
    
    
    private void SteeringByEyesSimple()
    {
        if (UnityEyetracker.et != null)
        {
            if (pointer.transform.position.x < gameObject.transform.position.x)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * speed;
            }
            else if (pointer.transform.position.x > gameObject.transform.position.x)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * speed;
            }

            if (Math.Abs(pointer.transform.position.x - gameObject.transform.position.x) < racketLengh / 5)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) * speed;
            }
        }
    }
    
    private void UpdateArrowsPosition()
    {
        float offsetBetweenArrows = 35;

        GameObject leftArrowbject = GameObject.FindWithTag("LeftArrow");
        BoxCollider2D leftArrowBoxColider2D = leftArrow.GetComponent<BoxCollider2D>();

        GameObject rightArrowbject = GameObject.FindWithTag("RightArrow");
        BoxCollider2D rightArrowBoxColider2D = rightArrow.GetComponent<BoxCollider2D>();

        leftArrowbject.transform.position = new Vector3(gameObject.transform.position.x - offsetBetweenArrows, gameObject.transform.position.y, leftArrow.transform.position.z);
        leftArrowBoxColider2D.transform.position = new Vector3(gameObject.transform.position.x - offsetBetweenArrows, gameObject.transform.position.y, leftArrow.transform.position.z);

        rightArrowbject.transform.position = new Vector3(gameObject.transform.position.x + offsetBetweenArrows, gameObject.transform.position.y, leftArrow.transform.position.z);
        rightArrowBoxColider2D.GetComponent<SpriteRenderer>().transform.position = new Vector3(gameObject.transform.position.x + offsetBetweenArrows, gameObject.transform.position.y, leftArrow.transform.position.z);
    }
    
    private void SteeringByArrows()
    {
        BoxCollider2D leftArrowRenderer = leftArrow.GetComponent<BoxCollider2D>();
        BoxCollider2D rightArrowRenderer = rightArrow.GetComponent<BoxCollider2D>();

        if (leftArrowRenderer.bounds.Intersects(pointer.GetComponent<BoxCollider2D>().bounds))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * speed;
        }
        else if (rightArrowRenderer.bounds.Intersects(pointer.GetComponent<BoxCollider2D>().bounds))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * speed;
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) * speed;
        }
        
        UpdateArrowsPosition();
    }
    
    private void SteeringByArrowsAdaptive()
    {
        BoxCollider2D leftArrowRenderer = leftArrow.GetComponent<BoxCollider2D>();
        BoxCollider2D rightArrowRenderer = rightArrow.GetComponent<BoxCollider2D>();

        if (leftArrowRenderer.bounds.Intersects(pointer.GetComponent<BoxCollider2D>().bounds))
        {
            var velocity = GetLeftVelocitySide();
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-velocity, 0) * speed;
        }
        else if (rightArrowRenderer.bounds.Intersects(pointer.GetComponent<BoxCollider2D>().bounds))
        {
            var velocity = GetRightVelocitySide();
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, 0) * speed;
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) * speed;
        }
        
        UpdateArrowsPosition();
    }
    
    
    private void InstantiateArrows()
    {
        leftArrow.SetActive(true);
        rightArrow.SetActive(true);

        if (settingsManager.sett.showArrows == Visible.Yes)
        {
            Color arrowsColor = GetArrowsColor();

            leftArrow.GetComponent<SpriteRenderer>().color = arrowsColor;
            rightArrow.GetComponent<SpriteRenderer>().color = arrowsColor;
        }
        else
        {
            Color arrowsTransparentColor = GetArrowsColor();
            arrowsTransparentColor.a = 0f;

            leftArrow.GetComponent<SpriteRenderer>().color = arrowsTransparentColor;
            rightArrow.GetComponent<SpriteRenderer>().color = arrowsTransparentColor;
        }
    }
    
    private Color GetArrowsColor()
    {
        switch (settingsManager.sett.steeringArrowsColor)
        {
            case SteeringArrowsColor.White:
                return Color.white;
            case SteeringArrowsColor.Yellow:
                return Color.yellow;
            case SteeringArrowsColor.Green:
                return Color.green;
            case SteeringArrowsColor.Red:
                return Color.red;
            default:
                return Color.white;
        }
    }

    public void ResetPosition()
    {
        if (settingsManager.sett.opposite.Equals(Opposite.Yes))
            transform.position = new Vector3(0.6f,98,-2.875f);
        else
            transform.position = new Vector3(0.6f,-98,-2.875f);
    }
}
