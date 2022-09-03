using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;



/// <summary>
/// Main playable character of the game. Can be player or AI controlled. Only needs movement inputs.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    // We can't use singletons if we want a multiplayer
    // SINGLETON
    //public static Character instance;
    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


    [SerializeField] private Rigidbody2D rigid2D = null;
    [FormerlySerializedAs("characterAnimator")] [SerializeField] public Animator faceAnimator = null;



    [Header("WICK")]
    [SerializeField] GameObject wickPrefab = null;
    [SerializeField] Transform wickFollowPoint = null;
    Meche attachedWick = null;


    // INPUTS
    Vector2 receivedMovementVector = new Vector2(0f, 0f);
    bool pauseInput = false;



    [Header("MOVEMENTS")]
    private Vector2 actualMovementVector = new Vector2(0f, 0f);
    [HideInInspector] public float baseSpeed = 300f;
    public float speed = 3f;
    public bool canReceiveInputs = true;




    [Header("FX")]
    [SerializeField] private ParticleSystem explosionFX = null;
    [SerializeField] private GameObject graphicsParent = null;



    [Header("AUDIO")]
    [SerializeField] private AudioSource explosionSFX = null;











    private void Start()
    {
        baseSpeed = speed;
        SpawnOwnWick();
    }
    private void FixedUpdate() => ManagementMovements();
    private void Update()
    {
        // PRESS PAUSE
        if (pauseInput)
            PressPause();
    }






    // INPUTS
    public void ReceiveMovementInputs(float x, float y)
    {
        if (canReceiveInputs)
            receivedMovementVector = new Vector2(x, y);
        else
            receivedMovementVector = new Vector2(0, 0);
    }
    public void ReceivePauseInput(bool pause) => pauseInput = pause;

    public void StopMoving()
    {
        rigid2D.velocity = Vector2.zero;
        canReceiveInputs = false;
    }
    public void GetControlsBack() => canReceiveInputs = true;






    #region MOVEMENTS
    void CalculateMovement() => actualMovementVector = Vector3.Normalize(receivedMovementVector) * speed * Time.deltaTime * LocalScaleAverage();
    // Used to base the character's speed on its scale, scale which should be 1 by default.
    float LocalScaleAverage()
    {
        Vector3 scale = transform.localScale;
        return (scale.x + scale.y + scale.z) / 3;
    }
    void ManagementMovements()
    {
        if (canReceiveInputs)
        {
            CalculateMovement();
            if (rigid2D)
                rigid2D.velocity = actualMovementVector;
        }
    }
    #endregion





    #region PAUSE
    public void PressPause()
    {
        if (PauseMenu.instance)
            PauseMenu.instance.PressEscape();
    }
    #endregion





    #region DIE
    public void Die() => StartCoroutine(Explode());
    IEnumerator Explode()
    {
        StopMoving();

        // KILL FLAMES
        foreach (Flame flame in FindObjectsOfType<Flame>())
            flame.Die();

        // FX
        if (explosionFX)
            explosionFX.Play();
        if (graphicsParent)
            graphicsParent.SetActive(false);


        // AUDIO
        if (explosionSFX)
            explosionSFX.Play();


        // CAMERA SHAKE
        // That's a complicated and not foolproof way at all to shake the camera
        GameManager.instance.currentLevel.GetComponent<Level>().cinemachineBrain.transform.GetChild(0).GetComponent<CameraShake>().ShakeCamera(10f, 0.3f);


        // We can't use a single wick instance for 2 players mode, working on a more foolproof and adaptable system
        // Meche.instance.Reset();
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /*
        // RESET WICKS
        foreach (Meche wick in FindObjectsOfType<Meche>())
            wick.Reset();
        */



        yield return new WaitForSeconds(1f);



        // RESET
        graphicsParent?.SetActive(true);
        GameManager.instance.currentLevel.GetComponent<Level>().RestartLevel();


        //transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        //transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        //transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
    }
    #endregion






    // WICK
    /// <summary>
    /// Spawns the wick of the player and sets it up for it.
    /// </summary>
    void SpawnOwnWick()
    {
        if (!attachedWick)
        {
            Meche newWick = Instantiate(wickPrefab).GetComponent<Meche>();
            newWick.followPoint = wickFollowPoint;
            newWick.SetFollowPlayer(true);
            newWick.followedPlayer = this;
            attachedWick = newWick;
        }
    }





    // EDITOR
    /// <summary>
    /// Automatically assign some references
    /// </summary>
    void GetMissingComponents()
    {
        if (!rigid2D && GetComponent<Rigidbody2D>())
            rigid2D = GetComponent<Rigidbody2D>();
    }
    private void OnValidate() => GetMissingComponents();

    //private void Awake() => instance = this;
}
