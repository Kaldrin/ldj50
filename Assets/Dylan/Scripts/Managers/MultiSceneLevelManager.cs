using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using JGDT.Audio.FadeInOut;
using UnityEngine.Events;

public class MultiSceneLevelManager : MonoBehaviour, IDataPersistence
{
    public static MultiSceneLevelManager instance;
    AsyncOperation async;
    public int lastLevelIndex;
    public int maxLevelIndex;
    public Character playerWhoTriggeredEndLevel;
    [SerializeField] private AudioFade ambianceAudioFade = null;
    [SerializeField] private AudioFade music = null;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(LoadMainMenuAtStart());
    }

    IEnumerator LoadMainMenuAtStart()
    {
        async = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        yield return async;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
    }

    public void LoadNextLevelAdditive(int nextLevelIndex, Character characterWhoTriggerd = null)
    {
        playerWhoTriggeredEndLevel = characterWhoTriggerd;
        StartCoroutine(LoadCoroutine(nextLevelIndex));
        lastLevelIndex = nextLevelIndex;
        if (lastLevelIndex > maxLevelIndex) maxLevelIndex = nextLevelIndex;
        DataPersistenceManager.instance.SaveGame();
    }

    IEnumerator LoadCoroutine(int nextSceneIndex)
    {
        int actualSceneIndex = SceneManager.GetActiveScene().buildIndex;
        async = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
        yield return async;
        yield return null;
        TeleportPlayerToLastLevel();
        GameManager.instance.currentLevel.GetComponent<Level>().StartLevel();
        yield return new WaitForSecondsRealtime(1);
        async = SceneManager.UnloadSceneAsync(actualSceneIndex);
        yield return async;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        GameManager.instance.currentLevel.GetComponent<Level>().SetMainMenuTrigger();
    }

    public void LoadData(GameData data)
    {
        this.lastLevelIndex = data.maxLevel;
        this.maxLevelIndex = data.maxLevel;
    }

    public void SaveData(ref GameData data)
    {
        data.maxLevel = this.maxLevelIndex;
        data.lastLevel = this.lastLevelIndex;
    }

    public void LoadLastLevel(Character characterWhoTriggerd = null)
    {
        playerWhoTriggeredEndLevel = characterWhoTriggerd;
        GameManager.instance.currentLevel.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        if (ambianceAudioFade)
            ambianceAudioFade.FadeOut();
        StartCoroutine(LoadLastLevelCoroutine(lastLevelIndex));
        Invoke("TriggerMusic", .5f);
    }

    IEnumerator LoadLastLevelCoroutine(int nextSceneIndex)
    {
        int actualSceneIndex = SceneManager.GetActiveScene().buildIndex;
        async = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
        yield return async;
        yield return null;
        TeleportPlayerToLastLevel();
        GameManager.instance.currentLevel.GetComponent<Level>().StartLevel();
        foreach (Meche meche in GameObject.FindObjectsOfType<Meche>()) meche.burning = true;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        //yield return new WaitForSecondsRealtime(1);
        async = SceneManager.UnloadSceneAsync(actualSceneIndex);
        yield return async;
        GameManager.instance.currentLevel.GetComponent<Level>().SetMainMenuTrigger();
    }

    void TeleportPlayerToLastLevel()
    {
        Vector2 nextLevelStartpoint = GameManager.instance.currentLevel.transform.GetChild(4).GetChild(0).position;
        foreach (Character player in GameObject.FindObjectsOfType<Character>())
        {
            if (player != playerWhoTriggeredEndLevel) player.transform.position = nextLevelStartpoint;
        }
        playerWhoTriggeredEndLevel = null;
    }

    IEnumerator LoadMainMenuCoroutine()
    {
        int actualSceneIndex = SceneManager.GetActiveScene().buildIndex;
        async = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        yield return async;
        yield return null;
        TeleportPlayerToMainMenu();
        GameManager.instance.currentLevel.GetComponent<Level>().StartLevel();
        foreach (Meche meche in GameObject.FindObjectsOfType<Meche>()) meche.burning = false;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
        //yield return new WaitForSecondsRealtime(1);
        async = SceneManager.UnloadSceneAsync(actualSceneIndex);
        yield return async;
        GameManager.instance.currentLevel.GetComponent<Level>().SetMainMenuTrigger();
    }

    void TriggerMusic()
    {
        if (music)
            music.audioSource.Play();
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadMainMenuCoroutine());
        if (music)
            music.audioSource.Stop();
        if (ambianceAudioFade)
            ambianceAudioFade.FadeIn();
    }

    void TeleportPlayerToMainMenu()
    {
        Vector2 mainMenuTeleportPoint = GameManager.instance.currentLevel.transform.GetChild(4).GetChild(0).position;
        foreach (Character player in GameObject.FindObjectsOfType<Character>())
        {
            if (player != playerWhoTriggeredEndLevel) player.transform.position = mainMenuTeleportPoint;
        }
        foreach (Meche wick in GameObject.FindObjectsOfType<Meche>())
        {
            wick.burning = false;
        }
        playerWhoTriggeredEndLevel = null;
    }
}
