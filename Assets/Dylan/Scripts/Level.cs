using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;




/// <summary>
/// Manages a level, in the level parent object
/// </summary>
public class Level : MonoBehaviour
{
    public GameObject cinemachineBrain;
    [SerializeField] Level nextLevel;
    [SerializeField] int nextLevelIndex;
    public UnityEvent actionsAtStart;
    [SerializeField] float actionsAtStartDelay = 0f;
    [SerializeField] bool firstLevel;
    [SerializeField] bool corridor;
    public GameObject startPoint;


    private void Awake()
    {
        //if (firstLevel)
        //{
        //SetAllObjects();
        if (!corridor)
            GameManager.instance.currentLevel = this;
        //}
    }
    private void OnEnable() => cinemachineBrain.SetActive(true);

    /// <summary>
    /// Trigger the specified events that should trigger at the beginning of the level
    /// </summary>
    void SetAllObjects() => Invoke("ActionsAtStart", actionsAtStartDelay);
    void ActionsAtStart() => actionsAtStart.Invoke();


    /// <summary>
    /// Changes to the next level and takes into account the player triggering the change to TP only the other one to the next level
    /// </summary>
    /// <param name="characterWhoTriggeredTheChange"></param>
    public void ChangeLevel(Character characterWhoTriggeredTheChange, Level followingLevel)
    {
        /*
        if (multiSceneDebug)
        {
            MultiSceneLevelManager.instance.LoadNextLevelAdditive(nextLevelIndex);
        }
        else
        {
            EndCurrentLevel();
            nextLevel.StartLevel();
            Invoke("RemovePreviousLevel", 2);
        }
        */
        EndCurrentLevel();

        Level newlyActivatedLevel = nextLevel;
        // If next level provided by the end level trigger, trigger it
        if (followingLevel && followingLevel != this)
        {
            followingLevel.StartLevel();
            newlyActivatedLevel = followingLevel;
        }
        // If not, trigger the default next level
        else
        {
            nextLevel.StartLevel();
            newlyActivatedLevel = nextLevel;
        }



        // Disable this level once the transition is done
        Invoke("RemovePreviousLevel", 1);


        // MOVE OTHER PLAYERS RO NEW LEVEL
        foreach (Character character in FindObjectsOfType<Character>())
            if (character != characterWhoTriggeredTheChange)
                character.transform.position = newlyActivatedLevel.startPoint.transform.position;

        // RESET OTHER PLAYERS WICKS
        KillFlames();
        ResetPlayerWicks();
        SetPlayerWicksToMove();
    }

    public void SetLevelTrigger()
    {
        if (transform.GetChild(4).childCount > 1) transform.GetChild(4).GetChild(1).gameObject.SetActive(true);
    }

    public void TimerBeforeSetMainMenuTrigger()
    {
        Invoke("MainMenuTrigger", 1);
    }

    void MainMenuTrigger()
    {
        transform.GetChild(4).GetChild(2).gameObject.SetActive(true);
    }

    void EndCurrentLevel() => cinemachineBrain.SetActive(false);
    /// <summary>
    /// Disables the current level after a duration to allow for the next one
    /// </summary>
    void RemovePreviousLevel() => gameObject.SetActive(false);

    public void StartLevel()
    {
        SetAllObjects();
        KillFlames();
        ResetPlayerWicks();
        SetPlayerWicksToMove();
    }

    public void RestartLevel()
    {
        // We can't use singletons if we want to have multiple players
        //Character.instance.transform.position = startPoint.transform.position;
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        foreach (Character character in FindObjectsOfType<Character>())
            character.transform.position = startPoint.transform.position;
        StartCoroutine(WaitBeforeRestart());
    }

    IEnumerator WaitBeforeRestart()
    {
        // If we have multiple bombs in 2 players mode we can't use an instance for the wick
        //Meche.instance.Reset();
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // RESET WICKS
        ResetPlayerWicks();


        // KILL FLAMES
        KillFlames();


        // RESET OTHER LEVEL ELEMENTS
        RestartLevelElements();


        yield return new WaitForSeconds(1);



        // If we are to have more than one player we cannot use instance, gotta replace that
        //Character.instance.GetControlsBack();
        //Meche.instance.SetFollowPlayer(true);
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // START AGAIN PLAYERS & WICKS
        if (!PauseMenu.instance.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            foreach (Character character in FindObjectsOfType<Character>())
                character.GetControlsBack();
        }
        SetPlayerWicksToMove();
    }

    /// <summary>
    /// Resets all players' wicks
    /// </summary>
    public void ResetPlayerWicks()
    {
        foreach (Meche wick in FindObjectsOfType<Meche>())
            if (wick.followedPlayer && wick.followPoint)
                wick.PlayerWickReset();
    }
    void ResetNonPlayerWicks()
    {
        foreach (Meche wick in FindObjectsOfType<Meche>())
            if (!wick.followedPlayer && !wick.followPoint)
                wick.Reset();
    }
    public void SetPlayerWicksToMove()
    {
        foreach (Meche wick in FindObjectsOfType<Meche>())
            if (wick.followedPlayer && wick.followPoint)
                wick.SetFollowPlayer(true);
    }
    void KillFlames()
    {
        foreach (Flame flame in FindObjectsOfType<Flame>())
            flame.Die();
    }

    void RestartLevelElements()
    {
        foreach (Door door in FindObjectsOfType<Door>())
            if (door)
                door.Reset();
        foreach (S_Switch switchs in FindObjectsOfType<S_Switch>())
            if (switchs)
                switchs.Reset();
        foreach (Oil oil in FindObjectsOfType<Oil>())
            if (oil)
                oil.Reset();
        foreach (HiddenDoorTimer hiddenDoorTimer in FindObjectsOfType<HiddenDoorTimer>())
            if (hiddenDoorTimer)
                hiddenDoorTimer.Reset();
        foreach (Torch torch in FindObjectsOfType<Torch>())
            if (torch)
                torch.Reset();
        foreach (CameraConfinerManager confinerManager in FindObjectsOfType<CameraConfinerManager>())
            if (confinerManager)
                confinerManager.Reset();
        foreach (HiddenCollisionTrigger collisionTrigger in FindObjectsOfType<HiddenCollisionTrigger>())
            if (collisionTrigger)
                collisionTrigger.Reset();
        foreach (GeneralTrigger generalTrigger in FindObjectsOfType<GeneralTrigger>())
            generalTrigger.Reset();
        ResetNonPlayerWicks();
    }
}
