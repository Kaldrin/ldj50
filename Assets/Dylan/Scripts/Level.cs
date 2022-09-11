using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    public GameObject cinemachineBrain;
    [SerializeField] int nextLevelIndex;
    public UnityEvent actionsAtStart;
    [SerializeField] bool firstLevel;
    [SerializeField] GameObject startPoint;
    [SerializeField] bool dontResetFlame = false;

    private void Start()
    {
        if (firstLevel)
        {
            SetAllObjects();
            GameManager.instance.currentLevel = gameObject;
        }
        //else StartLevel();
    }
<<<<<<< Updated upstream

    void SetAllObjects() => actionsAtStart?.Invoke();
=======
    //private void OnEnable() => cinemachineBrain.SetActive(true);

    /// <summary>
    /// Trigger the specified events that should trigger at the beginning of the level
    /// </summary>
    void SetAllObjects() => Invoke("ActionsAtStart", actionsAtStartDelay);
    void ActionsAtStart() => actionsAtStart.Invoke();

    /// <summary>
    /// Changes to the next level and takes into account the player triggering the change to TP only the other one to the next level
    /// </summary>
    /// <param name="characterWhoTriggeredTheChange"></param>
    /*public void ChangeLevel(Character characterWhoTriggeredTheChange, Level followingLevel)
    {
        MultiSceneLevelManager.instance.LoadNextLevelAdditive(nextLevelIndex);
        
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
    }*/

    public void SetCurrentLevel()
    {
        GameManager.instance.currentLevel = gameObject;
    }
>>>>>>> Stashed changes

    void EndCurrentLevel() => cinemachineBrain.SetActive(false);
    void RemovePreviousLevel() => gameObject.SetActive(false);

    public void StartLevel()
    {
        if(dontResetFlame) return;
        Debug.Log("Start Level");
        KillFlames();
        actionsAtStart?.Invoke();
        //gameObject.SetActive(true);
<<<<<<< Updated upstream
        GameManager.instance.currentLevel = gameObject;

        // Reset Flame State
=======
        ResetPlayerWicks();
        SetPlayerWicksToMove();
>>>>>>> Stashed changes
    }

    public void RestartLevel()
    {
        Character.instance.transform.position = startPoint.transform.position;
        StartCoroutine(WaitBeforeRestart());
    }

    IEnumerator WaitBeforeRestart()
    {
        Meche.instance.Reset();


        // Kill flames
        foreach (Flame flame in FindObjectsOfType<Flame>())
            flame.Die();

        RestartLevelElements();


        yield return new WaitForSeconds(1);



        Character.instance.GetControlsBack();
        Meche.instance.SetFollowPlayer(true);
        /*
        if (!firstLevel)
            Instantiate(GameManager.instance.flame);
            */
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
    }
}
