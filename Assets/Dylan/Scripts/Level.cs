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

    void SetAllObjects() => actionsAtStart?.Invoke();

    void EndCurrentLevel() => cinemachineBrain.SetActive(false);
    void RemovePreviousLevel() => gameObject.SetActive(false);

    public void StartLevel()
    {
        if(dontResetFlame) return;
        Debug.Log("Start Level");
        KillFlames();
        actionsAtStart?.Invoke();
        //gameObject.SetActive(true);
        GameManager.instance.currentLevel = gameObject;

        // Reset Flame State
        ResetPlayerWicks();
        SetPlayerWicksToMove();
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
