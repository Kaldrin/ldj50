using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class S_Switch : MonoBehaviour
{
    public UnityEvent action;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite spriteWhilePressed;

    [Header("FX")]
    [SerializeField] private ParticleSystem pressFX = null;
    [SerializeField] private AudioSource soundFx = null;
    
    
    
    
    private void OnEnable() {
        Reset();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        GetComponent<SpriteRenderer>().sprite = spriteWhilePressed;
        action.Invoke();
        GetComponent<Collider2D>().enabled = false;
        
        
        // FX
        if (pressFX)
            pressFX.Play();
        
        // AUDIO
        if (soundFx)
            soundFx.Play();
        //gameObject.SetActive(false);
    }

    public void Reset()
    {
        //gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
        GetComponent<Collider2D>().enabled = true;
    }
}
