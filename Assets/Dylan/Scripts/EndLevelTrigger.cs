using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    [SerializeField] int nextLevelIndex;

    private void OnTriggerEnter2D(Collider2D other) => MultiSceneLevelManager.instance.LoadNextLevelAdditive(nextLevelIndex);
    private void OnDrawGizmos() => Gizmos.DrawCube(transform.position, transform.localScale);
}
