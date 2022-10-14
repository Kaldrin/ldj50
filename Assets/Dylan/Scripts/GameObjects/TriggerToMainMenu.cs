using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToMainMenu : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GetComponent<Collider2D>().enabled = false;
        MultiSceneLevelManager.instance.LoadMainMenu();
    }
    private void OnDrawGizmos() => Gizmos.DrawCube(transform.position, transform.localScale);
}
