using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid2D = null;
    [FormerlySerializedAs("characterAnimator")] [SerializeField] public Animator faceAnimator = null;
    [HideInInspector] public Vector2 receivedMovementVector = new Vector2(0f, 0f);
    private Vector2 actualMovementVector = new Vector2(0f, 0f);
    public float speed = 3f;
    
    public static Character instance;

    bool canSendInputs = true;
    


    [Header("FX")]
    [SerializeField] private ParticleSystem explosionFX = null;
    [SerializeField] private GameObject graphicsParent = null;



    [Header("AUDIO")]
    [SerializeField] private AudioSource explosionSFX = null;
    
    
    
    
    
    
    
    
    
    
    
    void StopMoving()
    {
        canSendInputs = false;
        rigid2D.velocity = Vector2.zero;
    }
    public void GetControlsBack() => canSendInputs = true;
    private void FixedUpdate() => ManagementMovements();





    #region MOVEMENTS
    void CalculateMovement() => actualMovementVector = Vector3.Normalize(receivedMovementVector) * speed * Time.deltaTime;
    void ManagementMovements()
    {
        if (canSendInputs)
        {
            CalculateMovement();
            if (rigid2D)
                rigid2D.velocity = actualMovementVector;
        }
    }
    #endregion



    // EDITOR
    // Automatically assign some references
    void GetMissingComponents()
    {
        if (!rigid2D && GetComponent<Rigidbody2D>())
            rigid2D = GetComponent<Rigidbody2D>();
    }
    private void OnValidate() => GetMissingComponents();

    private void Awake() => instance = this;

    #region DIE
    public void Die()
    {
        // Play animation / Make Sound etc
        StartCoroutine(Explode());
    }

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
}
