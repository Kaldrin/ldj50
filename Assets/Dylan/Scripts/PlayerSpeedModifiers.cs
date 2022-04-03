using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedModifiers : MonoBehaviour
{
    Character chara;
    float startSpeed;
    [SerializeField] private ParticleSystem fastFX = null;
    [SerializeField] private ParticleSystem slowFX = null;
    
    

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

            chara.speed = startSpeed - startSpeed * 60 / 100;
        }
        else if (other.gameObject.tag == "FasterZone")
        {
            // FX
            if (fastFX && !fastFX.isEmitting)
                fastFX.Play();

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
            if (slowFX)
                slowFX.Stop();
            
            
            chara.speed = startSpeed;
        }
    }
}
