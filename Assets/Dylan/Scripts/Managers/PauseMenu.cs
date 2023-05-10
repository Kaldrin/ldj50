using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour, IDataPersistence
{
    public static PauseMenu instance;
    bool active = false;

    [SerializeField]
    AudioMixer audioMixer;

    [SerializeField]
    Slider mainSlider;

    [SerializeField]
    float volumeLevel;

    [SerializeField]
    Toggle fullscreenToggle;

    [SerializeField]
    int fullscreen = 1;
    GameObject lastselect;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadData(GameData data)
    {
        this.volumeLevel = data.volumeLevel;
        mainSlider.value = this.volumeLevel;
        this.fullscreen = data.fullscreen;
        if (data.fullscreen == 1)
        {
            Screen.fullScreen = true;
            fullscreenToggle.isOn = true;
        }
        else
        {
            Screen.fullScreen = false;
            fullscreenToggle.isOn = false;
        }
    }

    public void ChangeFullScreenValue()
    {
        if (!fullscreenToggle.isOn)
        {
            fullscreen = 0;
            Screen.fullScreen = false;
        }
        else if (fullscreenToggle.isOn)
        {
            fullscreen = 1;
            Screen.fullScreen = true;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.volumeLevel = this.volumeLevel;
        data.fullscreen = this.fullscreen;
    }

    public void PressEscape()
    {
        if (active)
            Unpause();
        else
            Pause();
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
        Arrow[] arrows = GameObject.FindObjectsOfType<Arrow>();
        for(int i = 0; i < arrows.Length; i++)
        {
            arrows[i].Pause();
        }
        ArrowSpawner[] arrowSpawners = GameObject.FindObjectsOfType<ArrowSpawner>();
        for(int i = 0; i < arrowSpawners.Length; i++)
        {
            arrowSpawners[i].Pause();
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
        Arrow[] arrows = GameObject.FindObjectsOfType<Arrow>();
        for(int i = 0; i < arrows.Length; i++)
        {
            arrows[i].UnPause();
        }
        ArrowSpawner[] arrowSpawners = GameObject.FindObjectsOfType<ArrowSpawner>();
        for(int i = 0; i < arrowSpawners.Length; i++)
        {
            arrowSpawners[i].Unpause();
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

    private void Start()
    {
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        fullscreenToggle.onValueChanged.AddListener(delegate { ChangeFullScreenValue(); });
    }

    void ValueChangeCheck()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(mainSlider.value) * 20);
        volumeLevel = mainSlider.value;
    }

    public void OpenMainPauseMenu()
    {
        transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(1).GetChild(1).gameObject);
    }

    public void GoToMainMenu()
    {
        Unpause();
        MultiSceneLevelManager.instance.LoadMainMenu();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
