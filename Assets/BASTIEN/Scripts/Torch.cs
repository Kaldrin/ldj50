using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Torch : MonoBehaviour
{
    [HideInInspector] public bool onFire = false;
    public UnityEvent action;
    
    [Header("FX")]
    [SerializeField] private ParticleSystem fireFX = null;
    [SerializeField] private ParticleSystem fireOnFX = null;


    
    public void Reset()
    {
        if (onFire)
            SetOnFire(false);
    }
    public void SetOnFire(bool state)
    {
        onFire = state;
        if (state)
        {
            action.Invoke();
            
            // FX
            if (fireFX)
                fireFX.Play();
            if (fireOnFX)
                fireOnFX.Play();
        }
        else
        {
            // FX
            if (fireFX)
                fireFX.Stop();
        }
    }
}
