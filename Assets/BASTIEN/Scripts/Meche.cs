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


    void StartFollowingPlayer()
    {
        if (characterToFollow && lineRenderer)
        {
            followingCharacter = true;
            
            // Set positions of line renderer
            Vector3[] positions = new Vector3[1] {characterToFollow.transform.position};
            lineRenderer.SetPositions(positions);
        }
            
    }

    void ManageFollow()
    {
        
    }

    void Calculate()
    {
        
    }
    void AddPoint()
    {
        
    }
    
    
    
    #region EDITOR
    void GetMissingComponents()
    {
        if (!lineRenderer && GetComponent<LineRenderer>())
            lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnValidate() => GetMissingComponents();
    #endregion

}
