using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using JGDT.Audio.FadeInOut;
using UnityEngine.Events;
using UnityEditor;
using System.Linq;

public class MultiSceneLevelManager : MonoBehaviour, IDataPersistence
{
    public static MultiSceneLevelManager instance;
    AsyncOperation async;
    public int lastLevelIndex;
    public int maxLevelIndex;
    [SerializeField] int actualLevelIndex;
    public Character playerWhoTriggeredEndLevel;
    [SerializeField] private AudioFade ambianceAudioFade = null;
    [SerializeField] private AudioFade music = null;
    [SerializeField] private string levelPath;

    [SerializeField] UnityEngine.Object[] scenes;

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

    public void LoadCorridor(int nextLevelIndex, Character characterWhoTriggered = null, EndingSide.Side sideOrientation = EndingSide.Side.UP, Vector3 pos = new Vector3())
    {
        playerWhoTriggeredEndLevel = characterWhoTriggered;
        lastLevelIndex = nextLevelIndex;
        if (lastLevelIndex > maxLevelIndex) maxLevelIndex = nextLevelIndex;
        DataPersistenceManager.instance.SaveGame();
        StartCoroutine(LoadCorridorCoroutine(nextLevelIndex, sideOrientation, pos));
    }

    IEnumerator LoadCorridorCoroutine(int nextLevelIndex, EndingSide.Side sideOrientation = EndingSide.Side.UP, Vector3 pos = new Vector3())
    {
        async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        yield return async;
        yield return null;

        CornersAndCorridorsManager.instance?.SetCorrespondingStructure(sideOrientation, pos, nextLevelIndex);
        TeleportPlayerToLastLevel();
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        GameObject.FindObjectOfType<Level>().cinemachineBrain.transform.GetChild(0).gameObject.SetActive(true);
        
        async = SceneManager.LoadSceneAsync(nextLevelIndex, LoadSceneMode.Additive);
        yield return async;
        yield return null;

        yield return new WaitForSeconds(.7f);
        async = SceneManager.UnloadSceneAsync(actualLevelIndex);
        yield return async;
        yield return null;
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextLevelIndex));
        actualLevelIndex = nextLevelIndex;
    }

    public void UnloadCorridor()
    {
        StartCoroutine(UnloadCorridorCoroutine());
    }

    IEnumerator UnloadCorridorCoroutine()
    {
        yield return new WaitForSeconds(1);
        async = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(1));
        yield return async;
    }

    public void LoadNextLevelAdditive(int nextLevelIndex, Character characterWhoTriggered = null, Vector3 pos = new Vector3())
    {
        playerWhoTriggeredEndLevel = characterWhoTriggered;
        lastLevelIndex = nextLevelIndex;
        StartCoroutine(LoadCoroutine(lastLevelIndex, pos));
        if (lastLevelIndex > maxLevelIndex) maxLevelIndex = lastLevelIndex;
        DataPersistenceManager.instance.SaveGame();
    }

    IEnumerator LoadCoroutine(int nextLevel, Vector3 pos = new Vector3())
    {
        actualLevelIndex = SceneManager.GetActiveScene().buildIndex;
        async = SceneManager.LoadSceneAsync(nextLevel, LoadSceneMode.Additive);

        yield return async;
        yield return null;

        GameManager.instance.currentLevel.transform.position = pos;
        TeleportPlayerToLastLevel();

        yield return new WaitForSecondsRealtime(1);
        async = SceneManager.UnloadSceneAsync(actualLevelIndex);
        yield return async;
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextLevel));
        actualLevelIndex = SceneManager.GetActiveScene().buildIndex;
        GameManager.instance.currentLevel.SetLevelTrigger();
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

    public void LoadLastLevel(Character characterWhoTriggerd = null, Vector3 pos = new Vector3())
    {
        playerWhoTriggeredEndLevel = characterWhoTriggerd;
        GameManager.instance.currentLevel.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        if (ambianceAudioFade)
            ambianceAudioFade.FadeOut();
        StartCoroutine(LoadLastLevelCoroutine(lastLevelIndex, pos));
        Invoke("TriggerMusic", .5f);
    }

    IEnumerator LoadLastLevelCoroutine(int nextSceneIndex, Vector3 pos = new Vector3())
    {
        int actualSceneIndex = SceneManager.GetActiveScene().buildIndex;
        async = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
        yield return async;
        yield return null;

        GameManager.instance.currentLevel.transform.position = pos;
        TeleportPlayerToLastLevel();
        foreach (Meche meche in GameObject.FindObjectsOfType<Meche>()) meche.burning = true;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
        //yield return new WaitForSecondsRealtime(1);

        yield return new WaitForSecondsRealtime(1);
        async = SceneManager.UnloadSceneAsync(actualSceneIndex);
        yield return async;
        GameManager.instance.currentLevel.SetLevelTrigger();
        actualLevelIndex = nextSceneIndex;
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
        foreach (Meche meche in GameObject.FindObjectsOfType<Meche>()) meche.burning = false;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));

        yield return new WaitForSecondsRealtime(1);
        async = SceneManager.UnloadSceneAsync(actualSceneIndex);
        yield return async;
        GameManager.instance.currentLevel.SetLevelTrigger();
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
        Vector2 mainMenuTeleportPoint = GameManager.instance.currentLevel.startPoint.transform.position;
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
