using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Torch object behaviour, works pretty much like a Zelda torch
/// </summary>
public class Torch : MonoBehaviour
{
    [HideInInspector] public bool onFire = false;
    [HideInInspector] public bool unconsume;
    public UnityEvent action;
    [SerializeField] private float duration = 2f;
    private float timer = 0f;

    [Header("FX")]
    [SerializeField] private ParticleSystem fireFX = null;
    [SerializeField] private ParticleSystem fireOnFX = null;
    [SerializeField] private ParticleSystem fireDeadFX = null;
    private Vector3 baseFXScale = new Vector3(0, 0, 0);

    [SerializeField] private AudioSource lightSFX = null;


    private void Awake() => baseFXScale = fireFX.transform.localScale;



    private void Update()
    {
        if (onFire)
        {
            if (!unconsume)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    SetOnFire(false);
                    action.Invoke();
                    if (fireDeadFX)
                        fireDeadFX.Play();
                }
                else
                    fireFX.transform.localScale = baseFXScale * (timer / duration);
            }
        }
    }



    public void Reset()
    {
        if (onFire)
            SetOnFire(false);
    }

    public void SetOnFire(bool state)
    {
        if (state)
        {
            action.Invoke();

            // FX
            if (fireFX)
                fireFX.Play();
            if (fireOnFX)
                fireOnFX.Play();

            // AUDIO
            if (lightSFX)
                lightSFX.Play();

            fireFX.transform.localScale = baseFXScale;
        }
        else
        {
            // FX
            if (fireFX)
                fireFX.Stop();
        }


        onFire = state;
        timer = duration;
    }
}
