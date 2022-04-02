using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    public GameObject cinemachineBrain;
    [SerializeField] Level nextLevel;
    public UnityEvent actionsAtStart;
    [SerializeField] bool firstLevel;
    [SerializeField] GameObject startPoint;

    private void Start() {
        if(firstLevel)
        {
            SetAllObjects();
            GameManager.instance.currentLevel = gameObject;
        }
    }

    void SetAllObjects()
    {
        actionsAtStart?.Invoke();
    }

    public void ChangeLevel()
    {
        EndCurrentLevel();
        nextLevel.StartLevel();
        Invoke("RemovePreviousLevel", 2);
    }

    void EndCurrentLevel()
    {
        cinemachineBrain.SetActive(false);
    }

    void RemovePreviousLevel()
    {
        gameObject.SetActive(false);
    }

    public void StartLevel()
    {
        actionsAtStart?.Invoke();
        gameObject.SetActive(true);
        GameManager.instance.currentLevel = gameObject;
        // Reset Flame State
    }

    public void RestartLevel()
    {
        Character.instance.transform.position = startPoint.transform.position;
        StartCoroutine(WaitBeforeRestart());
    }

    IEnumerator WaitBeforeRestart()
    {
        yield return new WaitForSeconds(1);
        Character.instance.GetComponent<PlayerController>().enabled = true;
        GameManager.instance.flame.GetComponent<Flame>().StartMoving();
    }
}
