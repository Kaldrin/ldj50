using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HiddenCollisionTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent actionToCallOnTriggerEnter;
    Collider2D col;

    private void Start() {
        col = GetComponent<Collider2D>();
    }
    public void Reset()
    {
        col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        col = GetComponent<Collider2D>();
        if(other.gameObject.CompareTag("Player"))
            actionToCallOnTriggerEnter?.Invoke();
        col.enabled = false;
    }
}