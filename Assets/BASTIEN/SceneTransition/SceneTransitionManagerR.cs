using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;





// Bastien BERNAND
// Reusable asset
// Last edited 27.10.2021
// Clean organized and optimzed

/// <summary>
/// Manages scene loading, transitions & animations. To place on a Canvas dedicated to scene transitions.
/// </summary>

// Originally made for UNITY 2020.1.15f1
// Last tested on UNITY 2021.1.26f1
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(GraphicRaycaster))]
[RequireComponent(typeof(CanvasScaler))]
public class SceneTransitionManagerR : MonoBehaviour
{
    // INSTANCE
    public static SceneTransitionManagerR instance;


    int sceneToLoadIndex = 0;
    [SerializeField] Animation animationComponentToUse = null;
    [SerializeField] string quitSceneAnimationName = "Quit";
    [SerializeField] List<KeyCode> keysToRestart = new List<KeyCode>() { KeyCode.LeftAlt, KeyCode.R};
    [SerializeField] bool disableAutoSetUp = false;



    // EDITOR
    int canvasSortingOrder = 200;
    string gameObjectName = "SceneTransitionManagerCanvas";




    void Awake() => instance = this;

    void Update()
    {
        if (enabled && isActiveAndEnabled)
            if (CheckIfAllKeysArePressed())
                RestartScene();
    }

    public void LoadSceneByIndex(int index)
    {
        GetMissingReferences();
        sceneToLoadIndex = index;
        // ANIMATION
        PlayQuitAnimation();
    }


    /// <summary>
    /// Quits the game, using the transition animation and the index -1 which <see cref="ProceedToLoadScene"/> will understand as quit the game.
    /// </summary>
    public void QuitGame()
    {
        sceneToLoadIndex = -1;
        // ANIMATION
        PlayQuitAnimation();
    }
    /// <summary>
    /// Restarts the current scene if all referenced keys of <see cref="keysToRestart"/>are pressed.
    /// </summary>
    public void RestartScene() => LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex);
    /// <summary>
    /// Immediately starts to load the scene of <see cref="sceneToLoadIndex"/> asynchronously. Usually shouldn't be called manually but rathered called by the quit animation of <see cref="animationComponentToUse"/>
    /// </summary>
    void ProceedToLoadScene() => StartCoroutine(ProceedToLoadSceneAsync());
    /// <summary>
    /// Coroutine used by <see cref="ProceedToLoadScene"/>
    /// </summary>
    IEnumerator ProceedToLoadSceneAsync()
    {
        if (sceneToLoadIndex == -1)
            Application.Quit();
        else
        {
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneToLoadIndex, LoadSceneMode.Single);
            while (!asyncOp.isDone)
                yield return null;
        }
    }

    /// <summary>
    /// Use by the Load secen functions to start the scene transition animation. This animation should trigger the <see cref="ProceedToLoadScene"/> function with an animation event
    /// </summary>
    void PlayQuitAnimation()
    {
        if (animationComponentToUse)
            animationComponentToUse.Play(quitSceneAnimationName, PlayMode.StopAll);
    }
    






    // EDITOR
    /// <summary>
    /// Immediately gets the missing references
    /// </summary>
    void GetMissingReferences()
    {
        if (!animationComponentToUse && GetComponent<Animation>())
            animationComponentToUse = GetComponent<Animation>();
        if (quitSceneAnimationName == "")
            quitSceneAnimationName = "Quit";
        if (sceneToLoadIndex < 0)
            sceneToLoadIndex = 0;
    }
    /// <summary>
    /// Quickly sets up the components to fit the use when it is dropped
    /// </summary>
    void SetUp()
    {
        gameObject.name = gameObjectName;

        if (GetComponent<Canvas>())
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = canvasSortingOrder;
        }
        if (GetComponent<CanvasGroup>())
        {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0;
        }
        if (GetComponent<CanvasScaler>())
        {
            CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
        }

        GameObject blackScreen = null;
        if (transform.childCount < 1)
        {
            blackScreen = new GameObject();
            blackScreen.transform.parent = transform;
        }
        else
            blackScreen = transform.GetChild(0).gameObject;
        blackScreen.name = "BlackScreen";
        if (!blackScreen.GetComponent<Image>())
            blackScreen.AddComponent<Image>();
        Image blackScreenImage = blackScreen.GetComponent<Image>();
        blackScreenImage.color = Color.black;
        blackScreenImage.raycastTarget = false;
        blackScreenImage.maskable = false;
        blackScreen.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        blackScreen.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1920);
        blackScreen.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1080);



        Debug.Log("Scene transition manager correctly set up, you just need to create the animations ('Open' and 'Quit') and add the 'ProceedToLoadScene' event at the end of the quit scene animation");
    }
    private void OnValidate()
    {
        GetMissingReferences();
        if (!disableAutoSetUp)
            SetUp();
    }





    // SECONDARY
    bool CheckIfAllKeysArePressed()
    {
        if (keysToRestart != null && keysToRestart.Count > 0)
        {
            bool allKeysPressed = true;

            for (int i = 0; i < keysToRestart.Count; i++)
                if (!Input.GetKey(keysToRestart[i]))
                    allKeysPressed = false;

            return allKeysPressed;
        }
        else
            return false;
    }
}
