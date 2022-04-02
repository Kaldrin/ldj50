using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Character characterToControl = null;


    [Header("INPUT AXIS")]
    [SerializeField] private string horizontal = "Horizontal";
    [SerializeField] private string vertical = "Vertical";
    
    
    // INPUTS
    private float xValue = 0f;
    private float yValue = 0f;
    
    
    
    void Update()
    {
        
    }
    
    
    
    
    #region GET INPUT VALUES
    
    #endregion
    
    
    
    
    #region SEND INPUT VALUES TO CHARACTER
    void SendCharacterMovements()
    {
        
    }
    #endregion

    
    
    
    #region EDITOR
    // Automatically assign some components
    void GetMissingComponents()
    {
        if (!characterToControl && GetComponent<Character>())
            characterToControl = GetComponent<Character>();
    }

    private void OnValidate() => GetMissingComponents();
    #endregion
}
