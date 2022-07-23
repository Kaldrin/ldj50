using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RotationInverseOfRB2DVel : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbodyToCheck = null;
    [SerializeField] Transform transformToCheck = null;
    private Vector3 lastTransformPosition = new Vector3(0, 0, 0);
    
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
        else if (transformToCheck)
        {
            if (lastTransformPosition != Vector3.zero)
            {
                Vector3 velocity = transformToCheck.position - lastTransformPosition;
                Vector3 normalizedSpeed = Vector3.Normalize(velocity);
                float newRotation = Mathf.Atan2(normalizedSpeed.y, normalizedSpeed.x) * Mathf.Rad2Deg;
                newRotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, newRotation + 90, Time.deltaTime * 15);
                Vector3 newEuler = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newRotation);
                transform.eulerAngles = newEuler;
            }
            if (lastTransformPosition != transform.position)
                lastTransformPosition = transformToCheck.position;
        }
    }
}
