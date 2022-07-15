using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public Slider mainSlider;
    bool active = false;
    [SerializeField] AudioMixer audioMixer;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void PressEscape()
    {
        if (active) Unpause();
        else Pause();
    }

    void Pause()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(1).GetChild(1).gameObject);
        Flame[] flames = GameObject.FindObjectsOfType<Flame>();
        for (int i = 0; i < flames.Length; i++)
        {
            flames[i].moving = false;
        }
        Torch[] torches = GameObject.FindObjectsOfType<Torch>();
        for (int i = 0; i < torches.Length; i++)
        {
            torches[i].unconsume = true;
        }
        Door[] doors = GameObject.FindObjectsOfType<Door>();
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].StopMoving();
        }
        Character[] characters = GameObject.FindObjectsOfType<Character>();
        for (int i = 0; i < characters.Length; i++)
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
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].GetControlsBack();
        }
        Flame[] flames = GameObject.FindObjectsOfType<Flame>();
        for (int i = 0; i < flames.Length; i++)
        {
            flames[i].moving = true;
        }
        Torch[] torches = GameObject.FindObjectsOfType<Torch>();
        for (int i = 0; i < torches.Length; i++)
        {
            torches[i].unconsume = false;
        }
        Door[] doors = GameObject.FindObjectsOfType<Door>();
        for (int i = 0; i < doors.Length; i++)
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

    public void OpenOptionsMenu()
    {
        transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(2).GetChild(2).gameObject);
    }

    public void SetVolume(float sliderValue)
    {
        Debug.Log(sliderValue);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
    public void ValueChangeCheck()
    {
        Debug.Log(mainSlider.value);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(mainSlider.value) * 20);
    }
    private void Start()
    {
        //SetVolume(.2f);
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    public void OpenMainPauseMenu()
    {
        transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(1).GetChild(1).gameObject);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
