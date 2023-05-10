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
    IEnumerator reset;
    IEnumerator open;
    [HideInInspector] public bool stopMoving = false;
    Vector2 goal;

    [Header("FX")]
    [SerializeField] private ParticleSystem dustFX = null;
    [Header("AUDIO")]
    [SerializeField] private AudioFade audioFade = null;



    void Start()
    {
        //hiddenSwitchList = listOfSwitches;
        initialPos = transform.position;
        Reset();
    }

    void OnEnable()
    {





        //Reset();
    }

    public void Reset()
    {
        hiddenSwitchList.Clear();
        for (int i = 0; i < listOfSwitches.Count; i++)
            hiddenSwitchList.Add(listOfSwitches[i]);
        reset = ResetPosition();
        StartCoroutine(reset);


        // AUDIO
        if (audioFade)
            audioFade.FadeOut();


        // FX
        if (dustFX)
            dustFX.Stop();
    }

    public void RemoveFromListOfSwitches(GameObject switchToRemove)
    {
        if (hiddenSwitchList.Contains(switchToRemove))
            hiddenSwitchList.Remove(switchToRemove);
        else if (listOfSwitches.Contains(switchToRemove))
            hiddenSwitchList.Add((switchToRemove));


        if (hiddenSwitchList.Count == 0)
        {
            open = Opening();
            StartCoroutine(open);
        }
        else
        {
            StartCoroutine(ResetPosition());
        }
    }

    public void StopMoving()
    {
        if (open != null)
            StopCoroutine(open);
    }

    public void MoveBack()
    {
        if (open != null)
        {
            open = Opening(false);
            StartCoroutine(open);
        }
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


        while (Vector2.Distance(transform.position, goal) > 0.05f)
        {
            Vector2 pos = Vector2.MoveTowards(transform.position, goal, openSpeed * Time.deltaTime);
            transform.position = pos;
        }
        transform.position = goal;


        // AUDIO
        audioFade?.FadeOut();

        // FX
        dustFX?.Stop();
    }

    IEnumerator Opening(bool fromStart = true)
    {
        if (reset != null)
        {
            StopCoroutine(reset);
            reset = null;
        }

        if (fromStart)
        {
            goal = Vector2.zero;
            if (directionWhenOpening.y > 0)
                goal = new Vector2(transform.position.x, transform.position.y + transform.localScale.y * directionWhenOpening.y);
            else if (directionWhenOpening.y < 0)
                goal = new Vector2(transform.position.x, transform.position.y - transform.localScale.y * -directionWhenOpening.y);
            else if (directionWhenOpening.x < 0)
                goal = new Vector2(transform.position.x - transform.localScale.x * -directionWhenOpening.x, transform.position.y);
            else if (directionWhenOpening.x > 0)
                goal = new Vector2(transform.position.x + transform.localScale.x * directionWhenOpening.x, transform.position.y);
        }


        // AUDIO
        if (audioFade)
        {
            audioFade.audioSource.Play();
            audioFade.FadeIn();
        }

        // FX
        if (dustFX)
            dustFX.Play();

        Debug.Log(goal);
        while (Vector2.Distance(transform.position, goal) > 0.05f)
        {
            Vector2 pos = Vector2.MoveTowards(transform.position, goal, openSpeed * Time.deltaTime);
            transform.position = pos;
            yield return null;
        }
        Debug.Log("Movement finished");
        transform.position = goal;


        // AUDIO
        if (audioFade)
            audioFade.FadeOut();

        // FX
        if (dustFX)
            dustFX.Stop();
    }



    IEnumerator ResetPosition()
    {
        if (open != null)
        {
            StopCoroutine(open);
            while (Vector2.Distance(transform.position, initialPos) != 0)
            {
                Vector2 pos = Vector2.MoveTowards(transform.position, initialPos, openSpeed * Time.deltaTime * 10);
                transform.position = pos;
                yield return new WaitForEndOfFrame();
            }


            audioFade?.FadeOut();
            dustFX?.Stop();

            open = null;
        }
    }
}
