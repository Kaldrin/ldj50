using System;
using System.Collections;
using System.Collections.Generic;
using JGDT.Audio.FadeInOut;
using UnityEngine;

public class FadeInOutMusic : MonoBehaviour
{
    [SerializeField] private AudioFade ambiance = null;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("truc");
        if (ambiance)
            ambiance.FadeIn();
    }
}
