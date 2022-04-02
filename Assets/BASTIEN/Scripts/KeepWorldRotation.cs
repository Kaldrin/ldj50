using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepWorldRotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotation = new Vector3(0, 0, 0);
    void Update()
    {
        if (transform.rotation.eulerAngles != rotation)
            transform.rotation.SetEulerAngles(rotation);
    }
}
