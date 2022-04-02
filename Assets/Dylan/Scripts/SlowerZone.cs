using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowerZone : MonoBehaviour
{
    Character chara;
    float startSpeed;

    private void OnTriggerEnter2D(Collider2D other) {
        chara = other.transform.parent.GetComponent<Character>();
        startSpeed = chara.speed;
        chara.speed -= chara.speed * 80/100;
    }

    private void OnTriggerExit2D(Collider2D other) {
        chara.speed = startSpeed;
    }
}
