using System.Collections;
using System.Collections.Generic;
using JGDT.Audio.FadeInOut;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] List<GameObject> listOfSwitches = new List<GameObject>();
    List<GameObject> hiddenSwitchList = new List<GameObject>();
    [SerializeField] float openSpeed;
    [SerializeField] Vector2 directionWhenOpening;
    Vector2 initialPos;


    [Header("AUDIO")] [SerializeField] private AudioFade audioFade = null;
    
    
    
    void OnEnable()
    {
        initialPos = transform.position;
        Reset();
    }

    public void Reset()
    {
        //hiddenSwitchList = listOfSwitches;
        listOfSwitches.Clear();
        for (int i = 0; i < listOfSwitches.Count; i++)
            hiddenSwitchList.Add(listOfSwitches[i]);
        //transform.position = initialPos;
        StartCoroutine(ResetPosition());
    }

    public void RemoveFromListOfSwitches(GameObject switchToRemove)
    {
        hiddenSwitchList.Remove(switchToRemove);
        if (hiddenSwitchList.Count == 0)
            StartCoroutine(Opening());
    }

    public void OpenAction()
    {
        Vector2 goal = Vector2.zero;
        if (directionWhenOpening == Vector2.up)
            goal = new Vector2(transform.position.x, transform.position.y + transform.localScale.y);
        else if (directionWhenOpening == Vector2.down)
            goal = new Vector2(transform.position.x, transform.position.y - transform.localScale.y);
        else if (directionWhenOpening == Vector2.left)
            goal = new Vector2(transform.position.x - transform.localScale.x, transform.position.y);
        else if (directionWhenOpening == Vector2.right)
            goal = new Vector2(transform.position.x + transform.localScale.x, transform.position.y);

        
        // AUDIO
        if (audioFade)
        {
            audioFade.audioSource.Play();
            audioFade.FadeIn();
        }
            
        
        while (Vector2.Distance(transform.position, goal) != 0)
        {
            Vector2 pos = Vector2.MoveTowards(transform.position, goal, openSpeed * Time.deltaTime);
            transform.position = pos;
        }
        
        // AUDIO
        if (audioFade)
            audioFade.FadeOut();
    }

    IEnumerator Opening()
    {
        Vector2 goal = Vector2.zero;
        if (directionWhenOpening.y > 0)
            goal = new Vector2(transform.position.x, transform.position.y + transform.localScale.y);
        else if (directionWhenOpening.y < 0)
            goal = new Vector2(transform.position.x, transform.position.y - transform.localScale.y);
        else if (directionWhenOpening.x < 0)
            goal = new Vector2(transform.position.x - transform.localScale.x, transform.position.y);
        else if (directionWhenOpening.x > 0)
            goal = new Vector2(transform.position.x + transform.localScale.x, transform.position.y);

        
        // AUDIO
        if (audioFade)
        {
            audioFade.audioSource.Play();
            audioFade.FadeIn();
        }
        
        
        while (Vector2.Distance(transform.position, goal) != 0)
        {
            Vector2 pos = Vector2.MoveTowards(transform.position, goal + directionWhenOpening, openSpeed * Time.deltaTime);
            transform.position = pos;
            yield return null;
        }
        
        // AUDIO
        if (audioFade)
            audioFade.FadeOut();
    }



    IEnumerator ResetPosition()
    {
        while (Vector2.Distance(transform.position, initialPos) != 0)
        {
            Vector2 pos = Vector2.MoveTowards(transform.position, initialPos, openSpeed * Time.deltaTime * 10);
            transform.position = pos;
            yield return new WaitForEndOfFrame();
        }
    }
}
