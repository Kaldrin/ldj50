using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPropagationCollider : MonoBehaviour
{
    private void OnEnable() => Invoke("Disable", 0.5f);
    void Disable() => gameObject.SetActive(false);
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Oil>() && !col.GetComponent<Oil>().onFire)
            col.GetComponent<Oil>().SetOnFire(true);
    }
}
