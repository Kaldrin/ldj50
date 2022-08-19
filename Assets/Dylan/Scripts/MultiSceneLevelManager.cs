using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MultiSceneLevelManager : MonoBehaviour
{
    public static MultiSceneLevelManager instance;
    AsyncOperation async;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void LoadNextLevelAdditive(int nextLevelIndex)
    {
        StartCoroutine(LoadCoroutine(nextLevelIndex));
    }

    IEnumerator LoadCoroutine(int nextSceneIndex)
    {
        int actualSceneIndex = SceneManager.GetActiveScene().buildIndex;
        async = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
        yield return async;
        SceneManager.UnloadSceneAsync(actualSceneIndex);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
    }
}
