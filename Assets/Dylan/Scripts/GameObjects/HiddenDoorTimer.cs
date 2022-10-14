using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDoorTimer : MonoBehaviour
{
    [SerializeField] float timeToWait;
    float currentTime;
    IEnumerator timer;


    public void Reset()
    {
        StopCoroutine(timer);
    }

    public void StartTimer()
    {
        timer = Timer();
        StartCoroutine(timer);
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(timeToWait);
        GetComponent<Door>().RemoveFromListOfSwitches(gameObject);
    }
}
