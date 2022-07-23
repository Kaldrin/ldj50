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
    // SINGLETON
    public static Character instance;


    [SerializeField] private Rigidbody2D rigid2D = null;
    [FormerlySerializedAs("characterAnimator")] [SerializeField] public Animator faceAnimator = null;


    // INPUTS
    Vector2 receivedMovementVector = new Vector2(0f, 0f);
    bool pauseInput = false;

    [Header("MOVEMENTS")]
    private Vector2 actualMovementVector = new Vector2(0f, 0f);
    public float speed = 3f;
    public bool canReceiveInputs = true;




    [Header("FX")]
    [SerializeField] private ParticleSystem explosionFX = null;
    [SerializeField] private GameObject graphicsParent = null;



    [Header("AUDIO")]
    [SerializeField] private AudioSource explosionSFX = null;












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
    void CalculateMovement() => actualMovementVector = Vector3.Normalize(receivedMovementVector) * speed * Time.deltaTime;
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


        // FX
        if (explosionFX)
            explosionFX.Play();
        if (graphicsParent)
            graphicsParent.SetActive(false);


        // AUDIO
        if (explosionSFX)
            explosionSFX.Play();


        //transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        //transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        //transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        //transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        //transform.GetChild(0)?.GetComponent<AudioSource>()?.Play();
        GameManager.instance.currentLevel.GetComponent<Level>().cinemachineBrain.transform.GetChild(0).GetComponent<CameraShake>().ShakeCamera(10f, 0.3f);
        Meche.instance.Reset();



        yield return new WaitForSeconds(1f);


        // RESET
        graphicsParent?.SetActive(true);
        GameManager.instance.currentLevel.GetComponent<Level>().RestartLevel();


        //transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        //transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        //transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
    }
    #endregion






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

    private void Awake() => instance = this;
}
