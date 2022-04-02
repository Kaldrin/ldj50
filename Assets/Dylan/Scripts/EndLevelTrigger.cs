using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        transform.parent.parent.GetComponent<Level>().ChangeLevel();
    }
}
