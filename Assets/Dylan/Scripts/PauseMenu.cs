using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;


/// <summary>
/// Pause menu manager script
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    bool active = false;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider mainSlider;





    void Awake() => instance = this;

    public void PressEscape()
    {
        if (active)
            Unpause();
        else
            Pause();
    }




    /// <summary>
    /// Triggers the pause of the game. Is enough by itself.
    /// </summary>
    void Pause()
    {
        // SET UP MENU AND FIRST SELECTED BUTTON
        transform.GetChild(0).gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(1).GetChild(1).gameObject);


        // STOPS ALL THE OBJECTS
        Flame[] flames = GameObject.FindObjectsOfType<Flame>();
        for (int i = 0; i < flames.Length; i++)
            flames[i].moving = false;
        Torch[] torches = GameObject.FindObjectsOfType<Torch>();
        for (int i = 0; i < torches.Length; i++)
            torches[i].unconsume = true;
        Door[] doors = GameObject.FindObjectsOfType<Door>();
        for (int i = 0; i < doors.Length; i++)
            doors[i].StopMoving();
        Character[] characters = GameObject.FindObjectsOfType<Character>();
        for (int i = 0; i < characters.Length; i++)
            characters[i].StopMoving();
        /*Animator[] animators = GameObject.FindObjectsOfType<Animator>();
        for(int i = 0; i < animators.Length; i++)
        {
            animators[i].speed = 0;
        }*/




        // TURN ON PAUSE STATE
        active = true;
    }



    /// <summary>
    /// Disables the pause of the game, objects start to move again. Enough by itself
    /// </summary>
    public void Unpause()
    {
        // DISABLES MENU AND UNSELECTS BUTTONS
        transform.GetChild(0).gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);



        Character[] characters = GameObject.FindObjectsOfType<Character>();
        for (int i = 0; i < characters.Length; i++)
            characters[i].GetControlsBack();
        Flame[] flames = GameObject.FindObjectsOfType<Flame>();
        for (int i = 0; i < flames.Length; i++)
            flames[i].moving = true;
        Torch[] torches = GameObject.FindObjectsOfType<Torch>();
        for (int i = 0; i < torches.Length; i++)
            torches[i].unconsume = false;
        Door[] doors = GameObject.FindObjectsOfType<Door>();
        for (int i = 0; i < doors.Length; i++)
            doors[i].MoveBack();
        /*Animator[] animators = GameObject.FindObjectsOfType<Animator>();
        for(int i = 0; i < animators.Length; i++)
        {
            animators[i].speed = 1;
        }*/



        // TURN OFF PAUSE STATE
        active = false;
    }










    
    // AUDIO VOLUME
    public void SetVolume(float sliderValue) => audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    private void Start() => mainSlider.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
    public void ValueChangeCheck() => audioMixer.SetFloat("MasterVolume", Mathf.Log10(mainSlider.value) * 20);




    // MENU SWITCH
    public void OpenMainPauseMenu()
    {
        transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(1).GetChild(1).gameObject);
    }
    public void OpenOptionsMenu()
    {
        transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(2).GetChild(2).gameObject);
    }




    // QUIT & MAIN MENU
    public void GoToMainMenu() => SceneTransitionManagerR.instance.LoadSceneByIndex(0);
    public void Quit() => SceneTransitionManagerR.instance.QuitGame();
}
