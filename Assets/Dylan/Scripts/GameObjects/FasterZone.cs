using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterZone : MonoBehaviour
{
    Character chara;
    float startSpeed;

    private void Start()
    {
        chara = Character.instance;
        startSpeed = chara.speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (chara.speed != startSpeed + chara.speed * 60 / 100)
            chara.speed = startSpeed + chara.speed * 60 / 100;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (chara.speed != startSpeed + chara.speed * 60 / 100)
            chara.speed = startSpeed;
    }
}
