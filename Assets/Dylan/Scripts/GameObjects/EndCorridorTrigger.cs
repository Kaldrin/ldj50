using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCorridorTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GetComponent<Collider2D>().enabled = false;
        GameManager.instance.currentLevel.transform.position = transform.position;
        GameManager.instance.currentLevel.cinemachineBrain.transform.GetChild(0).gameObject.SetActive(true);
        MultiSceneLevelManager.instance.UnloadCorridor();
        //MultiSceneLevelManager.instance.LoadNextLevelAdditive(CornersAndCorridorsManager.instance._nextLevelIndex, characterWhoTriggered: other.transform.parent.GetComponent<Character>(), transform.position);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
