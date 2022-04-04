using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedModifiers : MonoBehaviour
{
    Character chara;
    float startSpeed;
    [SerializeField] private ParticleSystem fastFX = null;
    [SerializeField] private ParticleSystem fastFX2 = null;
    [SerializeField] private ParticleSystem slowFX = null;
    [SerializeField] private ParticleSystem slowFX2 = null;
    
    

    void Awake()
    {
        chara = transform.parent.GetComponent<Character>();
        startSpeed = chara.speed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "SlowerZone")
        {
            // FX
            if (slowFX && !slowFX.isEmitting)
               slowFX.Play();
            if (slowFX2 && !slowFX2.isEmitting)
                slowFX2.Play();
            
            
            // ANIMATION
            chara.faceAnimator.SetBool("Slow", true);
            

            chara.speed = startSpeed - startSpeed * 60 / 100;
        }
        else if (other.gameObject.tag == "FasterZone")
        {
            // FX
            if (fastFX && !fastFX.isEmitting)
                fastFX.Play();
            if (fastFX2 && !fastFX2.isEmitting)
                fastFX2.Play();
            
            
            // ANIMATION
            chara.faceAnimator.SetBool("Fast", true);

            chara.speed = startSpeed + startSpeed * 60 / 100;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "SlowerZone" || other.gameObject.tag == "FasterZone")
        {
            // FX
            if (fastFX)
                fastFX.Stop();
            if (fastFX2)
                fastFX2.Stop();
            if (slowFX)
                slowFX.Stop();
            if (slowFX2)
                slowFX2.Stop();
            
            
            // ANIMATION
            chara.faceAnimator.SetBool("Fast", false);
            chara.faceAnimator.SetBool("Slow", false);
            
            
            chara.speed = startSpeed;
        }
    }
}
