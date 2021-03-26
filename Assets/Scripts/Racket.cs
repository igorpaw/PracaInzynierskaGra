﻿using System;
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
    
    private bool _lastToLeft = false;
    private bool _lastToRight = false;
    private bool _lastToLeftStop= false;
    private bool _lastToRightStop = false;
    private bool _lastUp = false;
    private bool _lastDown = false;
    private bool _startToRight = false;
    private bool _startToLeft = false;
    private bool _startToRightStop = false;
    private bool _startToLeftStop = false;
    
    private int _tier = 1;
    private Vector3 _target;
    private bool _steeringGesture = false;
    private bool _gestureMove = false;
    public int leftRight = 170;
    public int bottom = 60;
    public int activation = 40;

    public int minimal = 20;
    public int maximal = 120;

    public int tier1 = 30;
    public int tier2 = 60;
    public int tier3 = 90;
    public int tier4 = 120;
    public int tier5 = 150;
    
    public int moveSize = 20;
    public float racketLengh = 100f;

    private void Start()
    {
        lastTransform = pointer.transform;
        settingsManager = ScriptableObject.CreateInstance("SettingsManager") as SettingsManager;
        settingsManager.LoadData();
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
        Vector3 position = gameObject.transform.position;
        if (settingsManager.sett.opposite.Equals(Opposite.Yes))
            transform.position = new Vector3(gameObject.transform.position.x,-position.y,position.z);
        if (settingsManager.sett.steeringMethod.Equals(SteeringMethod.Arrows) ||
            settingsManager.sett.steeringMethod.Equals(SteeringMethod.ArrowsAdd))
        {
            Instantiate();
        }
        RacketSteering();
    }


    void FixedUpdate()
    {
        RacketSteering();
        if (_steeringGesture && _gestureMove)
        {
            var dir=(_target - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
            if (_target.x >= transform.position.x - 1 && _target.x <= transform.position.x + 1)
                _gestureMove = false;
        }
    }
    
    private void RacketSteering()
    {
        switch (settingsManager.sett.steeringMethod)
        {
            case SteeringMethod.Gesture:
                SteeringByGesture(false);
                _steeringGesture = true;
                break;
            case SteeringMethod.Arrows:
                SteeringByArrows();
                _steeringGesture = false;
                break;
            case SteeringMethod.Tier:
                SteeringByGesture(true);
                _steeringGesture = true;
                break;
            case SteeringMethod.EyesClosure:
                SteeringByEyesClosure();
                _steeringGesture = false;
                break;
            case SteeringMethod.EyesPosition:
                SteeringByEyesSimple();
                _steeringGesture = false;
                break;
            case SteeringMethod.MoveAdd:
                SteeringByEyesPositionAdaptive();
                _steeringGesture = false;
                break;
            case SteeringMethod.ArrowsAdd:
                SteeringByArrowsAdaptive();
                _steeringGesture = false;
                break;
            case SteeringMethod.GestCon:
                SteeringByGestureConstant();
                StopGesture();
                _steeringGesture = true;
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
            _startToRightStop = true;
            lastTransform = pointer.transform;
            return;
        }

        if (pointer.transform.position.x < 0 && pointer.transform.position.x > -40)
        {
            SetAllStopBool(false);
            _startToLeftStop = true;
            lastTransform = pointer.transform;
            return;
        }
        
        if (_startToRightStop)
        {
            MoveToRightStop();
        }

        if (_startToLeftStop)
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
            _startToRight = true;
            lastTransform = pointer.transform;
            return;
        }

        if (pointer.transform.position.x < 0 && pointer.transform.position.x > -40)
        {
            SetAllBool(false);
            _startToLeft = true;
            lastTransform = pointer.transform;
            return;
        }
            
        if (_startToRight)
        {
            MoveToRight(true);
        }

        if (_startToLeft)
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
            if (pointer.transform.position.x >= 0 && pointer.transform.position.x < activation)
            {
                SetAllBool(false);
                _startToRight = true;
                lastTransform = pointer.transform;
                return;
            }

            if (pointer.transform.position.x < 0 && pointer.transform.position.x > -activation)
            {
                SetAllBool(false);
                _startToLeft = true;
                lastTransform = pointer.transform;
                return;
            }
        }

        if (activationTiers && !_startToLeft && !_startToRight)
        {
            if (pointer.transform.position.x >= 0 && pointer.transform.position.x < tier1)
            {
                SetLength(5, true);
                return;
            }
            if (pointer.transform.position.x >= tier1 && pointer.transform.position.x < tier2)
            {
                SetLength(4, true);
                return;
            }
            if (pointer.transform.position.x >= tier2 && pointer.transform.position.x < tier3)
            {
                SetLength(3, true);
                return;
            }
            if (pointer.transform.position.x >= tier3 && pointer.transform.position.x < tier4)
            {
                SetLength(2, true);
                return;
            }
            if (pointer.transform.position.x >= tier4 && pointer.transform.position.x < tier5)
            {
                SetLength(1, true);
                return;
            }
            if (pointer.transform.position.x < 0 && pointer.transform.position.x > -tier1)
            {
                SetLength(5, false);
                return;
            }
            if (pointer.transform.position.x < -tier1 && pointer.transform.position.x > -tier2)
            {
                SetLength(4, false);
                return;
            }
            if (pointer.transform.position.x < -tier2 && pointer.transform.position.x > -tier3)
            {
                SetLength(3, false);
                return;
            }
            if (pointer.transform.position.x < -tier3 && pointer.transform.position.x > -tier4)
            {
                SetLength(2, false);
                return;
            }
            if (pointer.transform.position.x < -tier4 && pointer.transform.position.x > -tier5)
            {
                SetLength(1, false);
                return;
            }
        }
        

        if (_startToRight) MoveToRight(false);

        if (_startToLeft) MoveToLeft(false);
    }

    private void SetLength(int tier, bool toRight)
    {
        SetAllBool(false);
        if(toRight)
            _startToRight = true;
        else
        {
            _startToLeft = true;
        }
        lastTransform = pointer.transform;
        _tier = tier;
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
            _lastToRightStop = true;
            lastTransform = pointer.transform;
            return;
        }

        if (pointer.transform.position.x > leftRight && _lastToRightStop)
        {
            MoveDown();
        }
        
    }

    private void MoveToRight(bool constant)
    {
            if (pointer.transform.position.x < lastTransform.position.x &&
                pointer.transform.position.x < leftRight)
            {
                SetAllBool(false);
                lastTransform = pointer.transform;
                return;
            }

            if (pointer.transform.position.x >= lastTransform.position.x &&
                pointer.transform.position.x < leftRight)
            {
                _lastToRight = true;
                lastTransform = pointer.transform;
                return;
            }

            if (pointer.transform.position.x > leftRight && _lastToRight)
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
            _lastToLeftStop = true;
            lastTransform = pointer.transform;
            return;
        }

        if (pointer.transform.position.x < -leftRight && _lastToLeftStop)
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
            _lastToLeft = true;
            lastTransform = pointer.transform;
            return;
        }

        if (pointer.transform.position.x < -leftRight && _lastToLeft)
        {
            MoveUp(false, constant);
        }
        
    }

    private void MoveDown()
    {
        if (pointer.transform.position.y > -activation)
        {
            if (_lastDown)
            {
                if (pointer.transform.position.y <= lastTransform.position.y)
                {
                    _lastDown = true;
                    lastTransform = pointer.transform;
                    return;
                }
                SetAllStopBool(false);
                lastTransform= pointer.transform;
                return;
            }

            _lastDown = true;
            lastTransform = pointer.transform;
            return;
        }

        if (_lastDown)
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
            if (_lastUp)
            {
                if (pointer.transform.position.y >= lastTransform.position.y)
                {
                    _lastUp = true;
                    lastTransform = pointer.transform;
                    return;
                }
                SetAllBool(false);
                lastTransform= pointer.transform;
                return;
            }

            _lastUp = true;
            lastTransform = pointer.transform;
            return;
        }
        
        Vector3 position = transform.position;
        
        if (_lastUp)
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
                    _target = new Vector3(transform.position.x + moveSize*_tier, position.y,
                        position.z);
                    _gestureMove = true;
                }
                   
                else
                {
                    _target = new Vector3(transform.position.x - moveSize*_tier, position.y,
                        position.z);
                    _gestureMove = true;
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
        _lastUp = state;
        _lastToLeft = state;
        _lastToRight = state;
        _startToLeft = state;
        _startToRight = state;
    }
    
    private void SetAllStopBool(bool state)
    {
        _lastDown = state;
        _startToLeftStop = state;
        _startToRightStop = state;
        _startToLeftStop = state;
        _startToRightStop = state;
    }
    
    private void SteeringByEyesPositionAdaptive()
    {
        if (UnityEyetracker.et != null)
        {
            if (pointer.transform.position.x < gameObject.transform.position.x)
            {
                float velocity = GetVelocityValue();
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-velocity, 0) * speed;
            }
            else if (pointer.transform.position.x > gameObject.transform.position.x)
            {
                float velocity = GetVelocityValue();
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, 0) * speed;
            }

            if (Math.Abs(pointer.transform.position.x - gameObject.transform.position.x) < racketLengh / 5)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) * speed;
            }
        }
    }
    

    private float GetVelocityValue()
    {
        float distance = pointer.transform.position.x - transform.position.x;
        if (distance > maximal || distance < -maximal)
            return 1f;
        if (distance < minimal && distance > -minimal)
            return 0.25f;
        if(distance > 0)
            return distance / 120;
        return -distance / 120;
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

            if (Math.Abs(pointer.transform.position.x - gameObject.transform.position.x) < racketLengh)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) * speed;
            }
        }
    }
    
    private void UpdateArrowsPosition()
    {
        float offsetBetweenArrows = 225;

        GameObject leftArrowbject = GameObject.FindWithTag("LeftArrow");
        BoxCollider2D leftArrowBoxColider2D = leftArrow.GetComponent<BoxCollider2D>();

        GameObject rightArrowbject = GameObject.FindWithTag("RightArrow");
        BoxCollider2D rightArrowBoxColider2D = rightArrow.GetComponent<BoxCollider2D>();
        Vector3 position = gameObject.transform.position;

        leftArrowbject.transform.position = new Vector3(position.x - offsetBetweenArrows, -80, leftArrow.transform.position.z);
        leftArrowBoxColider2D.transform.position = new Vector3(position.x - offsetBetweenArrows, position.y, leftArrow.transform.position.z);

        rightArrowbject.transform.position = new Vector3(position.x + offsetBetweenArrows, -80, rightArrow.transform.position.z);
        rightArrowBoxColider2D.transform.position = new Vector3(position.x + offsetBetweenArrows, position.y, rightArrow.transform.position.z);
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
            var velocity = GetVelocityValue();
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-velocity, 0) * speed;
        }
        else if (rightArrowRenderer.bounds.Intersects(pointer.GetComponent<BoxCollider2D>().bounds))
        {
            var velocity = GetVelocityValue();
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, 0) * speed;
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0) * speed;
        }
        
        UpdateArrowsPosition();
    }
    
    
    private void Instantiate()
    {
        leftArrow.SetActive(true);
        rightArrow.SetActive(true);
        if (settingsManager.sett.showArrows == Visible.Yes)
        {
            Color arrowsColor = GetColor();
            arrowsColor.a = 0.75f;
            leftArrow.GetComponent<SpriteRenderer>().color = arrowsColor;
            rightArrow.GetComponent<SpriteRenderer>().color = arrowsColor;
        }
        else
        {
            Color arrowsTransparentColor = GetColor();
            arrowsTransparentColor.a = 0f;
            leftArrow.GetComponent<SpriteRenderer>().color = arrowsTransparentColor;
            rightArrow.GetComponent<SpriteRenderer>().color = arrowsTransparentColor;
        }
    }
    
    private Color GetColor()
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
            transform.position = new Vector3(0.0f,110,-2.875f);
        else
            transform.position = new Vector3(0.0f,-110,-2.875f);
    }
}
