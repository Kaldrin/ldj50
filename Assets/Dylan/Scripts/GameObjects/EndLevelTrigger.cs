using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndLevelTrigger : MonoBehaviour
{
    [HideInInspector] public int nextLevel;

    [HideInInspector] public bool spawnCorridor = false;
    [HideInInspector] public EndingSide.Side endingSide;

    [HideInInspector] public bool nextLevelInHierarchy = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GetComponent<Collider2D>().enabled = false;
        if (nextLevelInHierarchy)
        {
            if (spawnCorridor) MultiSceneLevelManager.instance.LoadCorridor(SceneManager.GetActiveScene().buildIndex + 1, other.transform.parent.GetComponent<Character>(), endingSide, transform.position);
            else MultiSceneLevelManager.instance.LoadNextLevelAdditive(SceneManager.GetActiveScene().buildIndex + 1, characterWhoTriggered: other.transform.parent.GetComponent<Character>(), transform.position);
        }
        else
        {
            if (spawnCorridor) MultiSceneLevelManager.instance.LoadCorridor(nextLevel, other.transform.parent.GetComponent<Character>(), endingSide, transform.position);
            else MultiSceneLevelManager.instance.LoadNextLevelAdditive(nextLevel, characterWhoTriggered: other.transform.parent.GetComponent<Character>(), transform.position);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}

public class EndingSide
{
    [Serializable]
    public enum Side
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
}