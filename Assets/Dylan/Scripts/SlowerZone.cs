using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// Script managing the slow zones interactions. When in a slow zone, a player is slowed down.
public class SlowerZone : MonoBehaviour
{
    // Can't use singletons if we want to have multiplayer. I switch to select the player who collided with the zone
    //Character chara;
    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //public float startSpeed;




    private void Start()
    {
        // Can't use singletons if we want to have multiplayer. I switch to select the player who collided with the zone
        //chara = Character.instance;
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //startSpeed = chara.speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Character character = other.transform.parent.GetComponent<Character>();
            if (character.speed != character.baseSpeed - character.speed * 80 / 100)
                character.speed = character.baseSpeed - character.speed * 80 / 100;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Character character = other.transform.parent.GetComponent<Character>();
            if (character.speed != character.baseSpeed - character.speed * 80 / 100)
                character.speed = character.baseSpeed;
        }
    }
}
