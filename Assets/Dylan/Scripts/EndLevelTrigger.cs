using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        transform.parent.parent.GetComponent<Level>().ChangeLevel();
    }
}
