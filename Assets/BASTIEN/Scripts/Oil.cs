using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : MonoBehaviour
{
    private bool onFire = false;
    private float propagationStartTime = 0f;
    private bool canPropagate = false;
    [SerializeField] private float propagationCooldown = 1f;

    [SerializeField] private GameObject oilPropagationAgentPrefab = null;
    [SerializeField] private ParticleSystem fireFX = null;


    private void Update()
    {
        if (onFire)
            if (!canPropagate && Time.time > propagationStartTime + propagationCooldown)
                TryToPropagate();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collisions");
        // PLAYER
        if (onFire && col.CompareTag("Player"))
            col.transform.parent.GetComponent<Character>().Die();
        // FLAME
        else if (!col.CompareTag("Player") && !onFire)
            SetOnFire(true);
    }

    void SetOnFire(bool state)
    {
        onFire = state;
        if (state)
        {
            propagationStartTime = Time.time;
            
            //FX
            if (fireFX)
                fireFX.Play();
        }
        else
        {
            if (fireFX)
                fireFX.Stop();
        }

        canPropagate = false;
    }



    #region FIRE PROPAGATION
    void TryToPropagate()
    {
        canPropagate = false;
        propagationStartTime = Time.time;
        Instantiate(oilPropagationAgentPrefab).transform.position = transform.position;
    }
    #endregion
}
