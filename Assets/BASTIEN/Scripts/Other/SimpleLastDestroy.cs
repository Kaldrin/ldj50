using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLastDestroy : MonoBehaviour
{
    [SerializeField] private float duration = 0;
    void Start() => Destroy(gameObject,duration);
}
