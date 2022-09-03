using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Scriptn that manages the oil object's behaviour, especially around fire propagation
/// </summary>
public class Oil : MonoBehaviour
{
    public bool onFire = false;
    private float propagationStartTime = 0f;
    private bool canPropagate = false;
    [SerializeField] private float propagationCooldown = 1f;
    [SerializeField] private float propagationRadius = 1.5f;

    [SerializeField] private GameObject oilPropagationCollider = null;
    [SerializeField] private ParticleSystem fireFX = null;
    [SerializeField] private GameObject playerColliderDetector = null;
    [SerializeField] private Collider2D oildCollider = null;
    [SerializeField] private LayerMask propagationMask = new LayerMask();





    private void Update()
    {
        // Try to propagate every X seconds
        if (onFire)
            if (!canPropagate && Time.time > propagationStartTime + propagationCooldown)
                TryToPropagate();
    }

    // When it's not ignited it can only collide with the Flames (2D physics rules settings)
    private void OnTriggerEnter2D(Collider2D col)
    {
        // PROPAGATION
        if (!onFire)
            SetOnFire(true);
    }






    public void Reset() => SetOnFire(false);
    
    
    
    
    public void SetOnFire(bool state)
    {
        onFire = state;
        // Now that it's on fire it can collide with a player to kill it, but it's a sub object to be precise
        playerColliderDetector.SetActive(state);
        if (state)
        {
            propagationStartTime = Time.time;
            oildCollider.enabled = false;
            
            // FX
            if (fireFX)
                fireFX.Play();
        }
        else
        {
            oildCollider.enabled = true;
            oilPropagationCollider.SetActive(false);
            
            // FX
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

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, propagationRadius, propagationMask);
        foreach (Collider2D col in cols)
            if (col)
            {
                if (col.GetComponent<Oil>() && !col.GetComponent<Oil>().onFire)
                    col.GetComponent<Oil>().SetOnFire(true);
                if (col.GetComponent<Torch>() && !col.GetComponent<Torch>().onFire)
                    col.GetComponent<Torch>().SetOnFire(true);
            }
    }
    #endregion
}
