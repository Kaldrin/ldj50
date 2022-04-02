using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    [SerializeField] GameObject cinemachineBrain;
    [SerializeField] Level nextLevel;
    public UnityEvent actionsAtStart;
    [SerializeField] bool firstLevel;

    private void Start() {
        if(firstLevel)
            SetAllObjects();
    }

    void SetAllObjects()
    {
        actionsAtStart?.Invoke();
    }

    public void ChangeLevel()
    {
        EndCurrentLevel();
        nextLevel?.StartLevel();
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
        gameObject.SetActive(true);
        // Reset Flame State
    }
}
