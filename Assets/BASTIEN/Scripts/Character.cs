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
    public float speed = 3f;

    public static Character instance;


    private void FixedUpdate() => ManagementMovements();





    #region MOVEMENTS
    void CalculateMovement() => actualMovementVector = Vector3.Normalize(receivedMovementVector) * speed * Time.deltaTime;
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

    private void Awake() {
        instance = this;
    }

    #region DIE
    public void Die()
    {
        GetComponent<PlayerController>().enabled = false;
        // Play animation / Make Sound etc
        GameManager.instance.currentLevel.GetComponent<Level>().RestartLevel();
    }
    #endregion
}
