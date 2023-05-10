using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : MonoBehaviour
{
    public bool onFire = false;
    private float propagationStartTime = 0f;
    public bool canPropagate = false;
    [SerializeField] private float propagationCooldown = 1f;
    [SerializeField] private float propagationRadius = 1.5f;

    [SerializeField] private GameObject oilPropagationCollider = null;
    [SerializeField] private ParticleSystem fireFX = null;
    [SerializeField] private GameObject playerColliderDetector = null;
    [SerializeField] private Collider2D oilCollider = null;
    [SerializeField] private LayerMask propagationMask = new LayerMask();
    bool initialState;



    private void Start()
    {
        initialState = onFire;
    }

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






    public void Reset() => CheckInitialState();
    
    private void CheckInitialState()
    {
        if(initialState) SetOnFire(true, false);
        else SetOnFire(false);
    }
    
    
    public void SetOnFire(bool state, bool deactivateOildCollider = true)
    {
        onFire = state;
        playerColliderDetector.SetActive(state);
        if (state)
        {
            propagationStartTime = Time.time;
            if(deactivateOildCollider) oilCollider.enabled = false;
            
            //FX
            if (fireFX)
                fireFX.Play();
        }
        else
        {
            oilCollider.enabled = true;
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
        propagationStartTime = Time.time;
        //oilPropagationCollider.SetActive(true);

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, propagationRadius, propagationMask);
        foreach (Collider2D col in cols)
            if (col)
            {
                if (col.GetComponent<Oil>() && !col.GetComponent<Oil>().onFire)
                    col.GetComponent<Oil>().SetOnFire(true);
                if (col.GetComponent<Torch>() && !col.GetComponent<Torch>().onFire)
                    col.GetComponent<Torch>().SetOnFire(true);
            }
        if(cols.Length > 0) canPropagate = false;
    }
    #endregion
}
