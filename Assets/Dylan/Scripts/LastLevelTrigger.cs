using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastLevelTrigger : MonoBehaviour
{
    private void OnDrawGizmos() => Gizmos.DrawCube(transform.position, transform.localScale);

    void OnTriggerEnter2D(Collider2D collider)
    {
        MultiSceneLevelManager.instance.LoadLastLevel();
    }
}
