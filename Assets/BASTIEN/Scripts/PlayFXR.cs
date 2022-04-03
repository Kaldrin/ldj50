using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Bastien BERNAND
// Reusable asset
// Last edited 04.11.2021

/// <summary>
/// Gives public functions with parameters to play FX from buttons or animation events
/// </summary>

// Originally made for UNITY 2021.1.26f1
// Last tested on UNITY 2021.1.26f1
public class PlayFXR : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> fxList = new List<ParticleSystem>();

    /// <summary>
    /// Plays the FX with the selected index in the list
    /// </summary>
    /// <param name="fxIndex"></param>
    public void PlayFX(int fxIndex)
    {
        if (fxIndex >= 0)
        {
            if (fxList != null && fxList.Count > fxIndex)
                if (fxList[fxIndex] != null)
                    fxList[fxIndex].Play();
        }
    }
}
