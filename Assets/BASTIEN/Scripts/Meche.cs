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
        if (startFollowingCharacterOnStart)
            StartFollowingPlayer();
    }

    private void Update()
    {
        // Follow character
        if (followingCharacter && characterToFollow)
            ManageFollow();
    }


    
    
    
    
    #region FOLLOW
    void StartFollowingPlayer()
    {
        if (characterToFollow && lineRenderer)
        {
            followingCharacter = true;
            
            // Set positions of line renderer
            Vector3[] positions = new Vector3[1] {characterToFollow.position};
            lineRenderer.SetPositions(positions);
        }
            
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
            //Vector3[] positions = lineRenderer.GetPositions();
            //lineRenderer.posi
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
