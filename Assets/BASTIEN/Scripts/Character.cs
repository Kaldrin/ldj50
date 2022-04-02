using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid2D = null;
    [HideInInspector] public Vector2 receivedMovementVector = new Vector2(0f, 0f);
    private Vector2 actualMovementVector = new Vector2(0f, 0f);


    private void Update()
    {
        ManagementMovements();
    }

    
    
    #region MOVEMENTS

    void CalculateMovement()
    {
        actualMovementVector = Vector3.Normalize(receivedMovementVector);
    }
    void ManagementMovements()
    {
        CalculateMovement();
        if (rigid2D)
            rigid2D.velocity = actualMovementVector;
    }
    #endregion
    
    
    
    // EDITOR
    // Automatically assign some references
    void GetMissingComponents()
    {
        if (!rigid2D && GetComponent<Rigidbody2D>())
            rigid2D = GetComponent<Rigidbody2D>();
    }
    private void OnValidate() => GetMissingComponents();
}
