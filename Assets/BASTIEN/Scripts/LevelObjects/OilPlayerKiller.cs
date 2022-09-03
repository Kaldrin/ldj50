using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Simple script for a sub object of the Oil object, the fire can kill the player on collision, it's that simple
/// </summary>
public class OilPlayerKiller : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            col.transform.parent.GetComponent<Character>().Die();
    }
}
