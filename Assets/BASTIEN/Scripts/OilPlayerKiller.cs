using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPlayerKiller : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            col.transform.parent.GetComponent<Character>().Die();
    }
}
