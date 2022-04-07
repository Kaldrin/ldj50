using System;
using System.Collections;
using System.Collections.Generic;
using JGDT.Audio.FadeInOut;
using UnityEngine;

public class FadeInOutMusic : MonoBehaviour
{
    [SerializeField] private AudioFade ambiance = null;
    [SerializeField] private AudioFade music = null;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (ambiance)
            ambiance.FadeIn();
        if (music)
            music.FadeOut();
    }
}
