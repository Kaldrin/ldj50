using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;




/// <summary>
/// Script for a wick. Not unique, can have multiple wicks if more than one player, but only one wick per player.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class Meche : MonoBehaviour
{
    //public static Meche instance;

    [FormerlySerializedAs("characterToFollow")] [SerializeField] public Transform followPoint = null;
    [SerializeField] public Character followedPlayer = null;

    [SerializeField] public LineRenderer lineRenderer = null;
    private float distanceToCreateNewPoint = 0.5f;
    private bool followingCharacter = false;
    [SerializeField] int amountOfPointsBeforeToSpawnFlame;
    int currentAmountOfPoints;
    bool flameExists;

    [Tooltip("The burning state of a wick means that if it becomes long enough it will ignite on its own, to allow for the wick to burn even after restarting levels and other stuff")]
    public bool burning = false;







    private void Awake()
    {
        /*
        // Can't use singletons because we will need multiple wicks for multiplayer
        if (originalMeche)
            instance = this;
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        */
    }

    private void Start()
    {
        // Clear line
        /*
        if (lineRenderer)
            lineRenderer.positionCount = 0;
        */
    }


    /// <summary>
    /// Resets the wick, clearing all of its points and centering it to the player it is set to follow.
    /// </summary>
    public void PlayerWickReset()
    {
        SetFollowPlayer(false);
        lineRenderer.positionCount = 0;
        currentAmountOfPoints = 0;
        flameExists = false;
    }
    /// <summary>
    /// Resets the wick to its non burned state. Only for levle objects wicks and not player wicks
    /// </summary>
    public void Reset() => SetAllPointsToZValue(0);


    private void Update()
    {
        // Follow character
        if (followingCharacter && followPoint)
            ManageFollow();
    }






    #region FOLLOW
    /// <summary>
    /// Enables or disables the following of the indicated player
    /// </summary>
    /// <param name="state"></param>
    public void SetFollowPlayer(bool state)
    {
        if (state)
        {
            if (followPoint && lineRenderer)
            {
                followingCharacter = true;

                // Set positions of line renderer
                AddPoint();
            }
        }
        else
            followingCharacter = false;
    }
    /// <summary>
    /// Manages the placement of points periodically when following the player. Waits for a certain distance between the last point and the player to place a new point
    /// </summary>
    void ManageFollow()
    {
        if (CalculateDistanceWithCharacter() >= distanceToCreateNewPoint)
            AddPoint();

        if (!followingCharacter)
            return;
        if (flameExists)
            return;
        // If is burning and is long enough, spawn the flame
        if (burning && currentAmountOfPoints >= amountOfPointsBeforeToSpawnFlame)
        {
            followingCharacter = true;

            // FLAME
            GameObject pref = Instantiate(GameManager.instance.flame);
            pref.transform.position = lineRenderer.GetPosition(0);
            pref.GetComponent<Flame>().standing = false;
            pref.GetComponent<Flame>().transform.position = lineRenderer.GetPosition(0);
            pref.GetComponent<Flame>().lineRendererToFollow = GetComponent<LineRenderer>();
            pref.GetComponent<Flame>().RestartMovingFromBeginning(0);
            flameExists = true;
        }
    }

    float CalculateDistanceWithCharacter()
    {
        float distance = 1f;
        if (followPoint && lineRenderer && lineRenderer.positionCount - 1 >= 0)
        {
            Vector3 lastPointPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
            Vector3 charaPos = followPoint.position;
            distance = Vector3.Distance(charaPos, lastPointPosition);
        }
        return distance;
    }
    void AddPoint()
    {
        if (followPoint && lineRenderer)
        {
            Vector3 charaPos = followPoint.position;
            // Set the Z pos to 0 to indicate it hasn't burnt
            charaPos = new Vector3(charaPos.x, charaPos.y, 0);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, charaPos);
            currentAmountOfPoints++;
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
    private void OnDrawGizmosSelected()
    {
        // For when we add wicks that are not attached to player, will be easier to draw them if their Z value doesn't go into the stars.
        if (!followedPlayer && !followPoint && !Application.isPlaying)
            SetAllPointsToZValue(0);
    }
    void SetAllPointsToZValue(int value)
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 currentPosition = lineRenderer.GetPosition(i);

            if (currentPosition.z != value)
            {
                currentPosition.z = value;
                lineRenderer.SetPosition(i, currentPosition);
            }
        }
    }
    #endregion

}
