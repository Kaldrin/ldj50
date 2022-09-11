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
    bool active = false;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider mainSlider;
    GameObject lastselect;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PressEscape()
    {
        if (active) Unpause();
        else Pause();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }
        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
    }

    void Pause()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
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
<<<<<<< Updated upstream
        }
=======
        
>>>>>>> Stashed changes
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
        PlayerPrefs.Save();
    }

    public void OpenOptionsMenu()
    {
        transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(2).GetChild(2).gameObject);
    }

    private void Start()
    {
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    public void ValueChangeCheck()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(mainSlider.value) * 20);
        PlayerPrefs.SetFloat("soundVolume", mainSlider.value);
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
