using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRendererToFollow = null;
    [SerializeField] private float speed = 0.002f;
    [SerializeField] private bool startOnStart = false;
    private bool moving = false;
    
    
    private int currentSectionIndex = 0;
    private float currentSectionLength = 0f;
    private float currentSectionRanDistance = 0f;
    private Vector3 currentPointPos = new Vector3(0f, 0f, 0f);
    private Vector3 nextpoinsPos = new Vector3(0f, 0f, 0f);


    
    
    
    
    
    private void Start()
    {
        if (startOnStart)
            Invoke("RestartMovingFromBeginning", 3f);
    }
    private void Update()
    {
        UpdateMoving();
    }

    
    
    

    #region START & STOP
    public void SetMoving(bool state) => moving = state;
    void RestartMovingFromBeginning()
    {
        if (lineRendererToFollow && lineRendererToFollow.positionCount > 5)
        {
            currentSectionIndex = 0;
            //transform.position = lineRendererToFollow.GetPosition(0);
            NewSection(0, 0);
            SetMoving(true);
        }
        //else
            //Invoke("RestartMovingFromBeginning", 1f);
    }
    #endregion
    
    
    
    
    
    
    #region MOVING
    void NewSection(int newSectionIndex, float startValue)
    {
        currentPointPos = lineRendererToFollow.GetPosition(currentSectionIndex);
        nextpoinsPos = lineRendererToFollow.GetPosition(currentSectionIndex + 1);
        currentSectionLength = CalculateCurrentSectionLength();
        currentSectionRanDistance = startValue;
        UpdateGradient();
    }
    
    float CalculateCurrentSectionLength()
    {
        float distance = 0f;
        if (lineRendererToFollow)
            distance = Vector3.Distance(currentPointPos, nextpoinsPos);

        return distance;
    }
    void UpdateMoving()
    {
        if (moving)
        {
            currentSectionRanDistance += speed * Time.deltaTime;
            // If reached the end of the section
            if (currentSectionRanDistance >= currentSectionLength)
            {
                currentSectionIndex++;
                if (currentSectionIndex == lineRendererToFollow.positionCount - 1)
                    TouchPlayer();
                else
                    NewSection(currentSectionIndex, currentSectionRanDistance - currentSectionLength);
            }

            SetPos();
        }
    }
    void SetPos()
    {
        float currentSectionProgressNormalized = currentSectionRanDistance / currentSectionLength;
        Vector3 nextSectionAddedVector = nextpoinsPos - currentPointPos;
        nextSectionAddedVector *= currentSectionProgressNormalized;
        Vector3 newPosition = currentPointPos + nextSectionAddedVector;
        try
        {
            transform.position = newPosition;
        }
        catch { }
    }

    void UpdateGradient()
    {
        Gradient newGradient = new Gradient();
        float ratio = (float)((float) currentSectionIndex / (float) lineRendererToFollow.positionCount);
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[4];
        GradientColorKey[] colorKeys = new GradientColorKey[2]{new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 0f)};
        
        // Leftest key
        alphaKeys[0].alpha = 0f;
        alphaKeys[0].time = 0;
        
        // Left key
        alphaKeys[1].alpha = 0f;
        alphaKeys[1].time = ratio + (1 / lineRendererToFollow.positionCount) * 5;
        
        // Right key
        alphaKeys[2].alpha = 1f;
        alphaKeys[2].time = ratio + (1 / lineRendererToFollow.positionCount) * 10;
        
        // Rightest key
        alphaKeys[3].alpha = 1f;
        alphaKeys[3].time = 1f;

        
        newGradient.SetKeys(colorKeys,alphaKeys);
        lineRendererToFollow.colorGradient = newGradient;
    }
    #endregion




    void TouchPlayer()
    {
        Debug.Log("BOOM");
        moving = false;
    }
}
