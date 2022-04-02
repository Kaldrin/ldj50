using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationInverseOfRB2DVel : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbodyToCheck = null;

    private void Update()
    {
        if (rigidbodyToCheck && Vector3.Magnitude(rigidbodyToCheck.velocity) > 0.1f)
        {
            Vector3 normalizedSpeed = Vector3.Normalize(rigidbodyToCheck.velocity);
            float newRotation = Mathf.Atan2(normalizedSpeed.y, normalizedSpeed.x) * Mathf.Rad2Deg;
            newRotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, newRotation + 90, Time.deltaTime * 15);
            Vector3 newEuler = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newRotation);
            transform.eulerAngles = newEuler;
        }
    }
}
