using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class S_Switch : MonoBehaviour
{
    public UnityEvent action;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite spriteWhilePressed;
    
    private void OnTriggerEnter2D(Collider2D other) {
        GetComponent<SpriteRenderer>().sprite = spriteWhilePressed;
        action.Invoke();
        //gameObject.SetActive(false);
    }

    public void Reset()
    {
        //gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }
}
