using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    bool active = false;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void PressEscape()
    {
        if(active) Unpause();
        else Pause();
    }
    
    void Pause()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(2).gameObject);
        Flame[] flames = GameObject.FindObjectsOfType<Flame>();
        for(int i = 0; i < flames.Length; i++)
        {
            flames[i].moving = false;
        }
        Torch[] torches = GameObject.FindObjectsOfType<Torch>();
        for(int i = 0; i < torches.Length; i++)
        {
            torches[i].unconsume = true;
        }
        Door[] doors = GameObject.FindObjectsOfType<Door>();
        for(int i = 0; i < doors.Length; i++)
        {
            doors[i].StopMoving();
        }
        Character[] characters = GameObject.FindObjectsOfType<Character>();
        for(int i = 0; i < characters.Length; i++)
        {
            characters[i].StopMoving();
        }
        /*Animator[] animators = GameObject.FindObjectsOfType<Animator>();
        for(int i = 0; i < animators.Length; i++)
        {
            animators[i].speed = 0;
        }*/
        active = true;
    }

    public void Unpause()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        Character[] characters = GameObject.FindObjectsOfType<Character>();
        for(int i = 0; i < characters.Length; i++)
        {
            characters[i].GetControlsBack();
        }
        Flame[] flames = GameObject.FindObjectsOfType<Flame>();
        for(int i = 0; i < flames.Length; i++)
        {
            flames[i].moving = true;
        }
        Torch[] torches = GameObject.FindObjectsOfType<Torch>();
        for(int i = 0; i < torches.Length; i++)
        {
            torches[i].unconsume = false;
        }
        Door[] doors = GameObject.FindObjectsOfType<Door>();
        for(int i = 0; i < doors.Length; i++)
        {
            doors[i].MoveBack();
        }
        /*Animator[] animators = GameObject.FindObjectsOfType<Animator>();
        for(int i = 0; i < animators.Length; i++)
        {
            animators[i].speed = 1;
        }*/
        active = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
