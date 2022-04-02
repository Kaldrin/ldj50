using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Meche : MonoBehaviour
{
    [SerializeField] Transform characterToFollow = null;
    [SerializeField] private LineRenderer lineRenderer = null;
    [SerializeField] private bool startFollowingCharacterOnStart = false;
    private float distanceToCreateNewPoint = 0.5f;
    private bool followingCharacter = false;

    
    
    
    private void Start()
    {
        // Clear line
        if (lineRenderer)
            lineRenderer.positionCount = 0;
        
        if (startFollowingCharacterOnStart)
            SetFollowPlayer(true);
    }


    public void Reset()
    {
        SetFollowPlayer(false);
        lineRenderer.positionCount = 0;
    }

    
    private void Update()
    {
        // Follow character
        if (followingCharacter && characterToFollow)
            ManageFollow();
    }


    
    
    
    
    #region FOLLOW
    void SetFollowPlayer(bool state)
    {
        if (state)
        {
            if (characterToFollow && lineRenderer)
            {
                followingCharacter = true;
            
                // Set positions of line renderer
                AddPoint();
            }
        }
        else
            followingCharacter = false;
    }
    
    void ManageFollow()
    {
        if (CalculateDistanceWithCharacter() >= distanceToCreateNewPoint)
            AddPoint();
    }

    float CalculateDistanceWithCharacter()
    {
        float distance = 0f;
        if (characterToFollow && lineRenderer)
        {
            Vector3 lastPointPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
            Vector3 charaPos = characterToFollow.position;
            distance = Vector3.Distance(charaPos, lastPointPosition);
        }
        return distance;
    }
    void AddPoint()
    {
        if (characterToFollow && lineRenderer)
        {
            Vector3 charaPos = characterToFollow.position;
            // Set the Z pos to 0 to indicate it hasn't burnt
            charaPos = new Vector3(charaPos.x, charaPos.y, 0);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, charaPos);
        }
    }
    #endregion



    




    #region EDITOR
    void GetMissingComponents()
    {
        if (!lineRenderer && GetComponent<LineRenderer>())
            lineRenderer = GetComponent<LineRenderer>();
    }
    private void OnValidate() => GetMissingComponents();
    #endregion

}
