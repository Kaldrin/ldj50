using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : MonoBehaviour
{
    public bool onFire = false;
    private float propagationStartTime = 0f;
    private bool canPropagate = false;
    [SerializeField] private float propagationCooldown = 1f;

    [SerializeField] private GameObject oilPropagationCollider = null;
    [SerializeField] private ParticleSystem fireFX = null;
    [SerializeField] private GameObject playerColliderDetector = null;
    [SerializeField] private Collider2D oildCollider = null;
    [SerializeField] private LayerMask propagationMask = new LayerMask();


    private void Update()
    {
        if (onFire)
            if (!canPropagate && Time.time > propagationStartTime + propagationCooldown)
                TryToPropagate();
    }

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
        playerColliderDetector.SetActive(state);
        if (state)
        {
            propagationStartTime = Time.time;
            oildCollider.enabled = false;
            
            //FX
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
        //oilPropagationCollider.SetActive(true);

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 1.5f, propagationMask);
        foreach (Collider2D col in cols)
            if (col && col.GetComponent<Oil>() && !col.GetComponent<Oil>().onFire)
                col.GetComponent<Oil>().SetOnFire(true);
    }
    #endregion
}
