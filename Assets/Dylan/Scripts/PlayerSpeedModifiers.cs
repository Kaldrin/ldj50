using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedModifiers : MonoBehaviour
{
    Character chara;
    float startSpeed;

    void Awake()
    {
        chara = transform.parent.GetComponent<Character>();
        startSpeed = chara.speed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "SlowerZone")
            chara.speed = startSpeed - startSpeed * 80 / 100;
        else if (other.gameObject.tag == "FasterZone")
            chara.speed = startSpeed + startSpeed * 60 / 100;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "SlowerZone" || other.gameObject.tag == "FasterZone")
            chara.speed = startSpeed;
    }
}
