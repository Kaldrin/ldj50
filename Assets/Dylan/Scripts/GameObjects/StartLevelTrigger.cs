using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


[RequireComponent(typeof(Collider2D))]
public class StartLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        /*if (SceneManager.GetSceneByBuildIndex(1).isLoaded)
            MultiSceneLevelManager.instance.UnloadCorridor();
        */
        GameManager.instance.currentLevel = transform.parent.parent.GetComponent<Level>();
        GameManager.instance.currentLevel.SetLevelTrigger();
        GameManager.instance.currentLevel.StartLevel();
        GameManager.instance.currentLevel.cinemachineBrain.transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<Collider2D>().enabled = false;
        
        if (other.tag == "Player")
        {
            /*foreach (ILevelStart objectToStart in FindInterfacesOfType<ILevelStart>())
            {
                objectToStart.LevelStart();
            }*/
            ArrowSpawner[] arrowSpawners = GameObject.FindObjectsOfType<ArrowSpawner>();
            for (int i = 0; i < arrowSpawners.Length; i++)
            {
                arrowSpawners[i].LevelStart();
            }
        }
    }

    IEnumerable<T> FindInterfacesOfType<T>()
    {
        Debug.Log(SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(go => go.GetComponentsInChildren<T>()).Count());
        return SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(go => go.GetComponentsInChildren<T>());
    }
}
