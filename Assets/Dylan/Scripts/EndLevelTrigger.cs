using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
<<<<<<< Updated upstream
=======
    [SerializeField] Color color = Color.gray;
    [SerializeField] Level nextLevel = null;
    bool triggered = false;
    [SerializeField] int specificPlayerIndexForSpecialActions = 1;
    [SerializeField] public UnityEvent actionsIfSpecificPlayerIndexTriggeredIt;
    [SerializeField] float specialActionsDelay = 0f;
>>>>>>> Stashed changes
    [SerializeField] int nextLevelIndex;

    private void OnTriggerEnter2D(Collider2D other) => MultiSceneLevelManager.instance.LoadNextLevelAdditive(nextLevelIndex);
    private void OnDrawGizmos() => Gizmos.DrawCube(transform.position, transform.localScale);
}
