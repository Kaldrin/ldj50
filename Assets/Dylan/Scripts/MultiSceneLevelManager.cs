using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MultiSceneLevelManager : MonoBehaviour, IDataPersistence
{
    public static MultiSceneLevelManager instance;
    AsyncOperation async;
    public int lastLevelIndex;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void LoadNextLevelAdditive(int nextLevelIndex, Action nextVoid = null)
    {
        StartCoroutine(LoadCoroutine(nextLevelIndex, nextVoid));
        if(nextLevelIndex != 2) lastLevelIndex = nextLevelIndex;
        DataPersistenceManager.instance.SaveGame();
    }

    IEnumerator LoadCoroutine(int nextSceneIndex, Action nextVoid = null)
    {
        int actualSceneIndex = SceneManager.GetActiveScene().buildIndex;
        async = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
        yield return async;
        async = SceneManager.UnloadSceneAsync(actualSceneIndex);
        yield return async;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        if(nextVoid != null) nextVoid.Invoke();
    }

    public void LoadData(GameData data)
    {
        this.lastLevelIndex = data.maxLevel;
    }

    public void SaveData(ref GameData data)
    {
        data.maxLevel = this.lastLevelIndex;
    }

    public void LoadLastLevel()
    {
        LoadNextLevelAdditive(lastLevelIndex, TeleportPlayerToLastLevel);
    }

    void TeleportPlayerToLastLevel()
    {
        Vector2 nextLevelStartpoint = GameObject.FindObjectOfType<Level>().transform.GetChild(4).GetChild(0).position;
        Character[] players = GameObject.FindObjectsOfType<Character>();
        foreach(Character player in players)
        {
            player.transform.position = nextLevelStartpoint;
        }
    }

    public void LoadMainMenu()
    {
        LoadNextLevelAdditive(2, TeleportPlayerToMainMenu);
    }

    void TeleportPlayerToMainMenu()
    {
        Vector2 mainMenuTeleportPoint = GameObject.FindObjectOfType<Level>().transform.GetChild(4).GetChild(1).position;
        Debug.Log(mainMenuTeleportPoint);
        Character[] players = GameObject.FindObjectsOfType<Character>();
        foreach(Character player in players)
        {
            player.transform.position = mainMenuTeleportPoint;
        }
    }
}
