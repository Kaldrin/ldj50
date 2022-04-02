using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformXAlongSpeed : MonoBehaviour
{
    [SerializeField] private Vector2 xDeformLimits = new Vector2(0.8f, 1);
    [SerializeField] private Vector2 yDeformLimits = new Vector2(1, 1.2f);
    [SerializeField] private Rigidbody2D rb2D = null;
    [SerializeField] private Vector2 velocityLimits = new Vector2(0f, 5f);
    //private Vector2 baseScale = new Vector2(1, 1);
     
    private void Update()
    {
        if (rb2D)
        {
            float speed = Vector3.Magnitude(rb2D.velocity);
            if (speed > velocityLimits.y)
                speed = velocityLimits.y;
            float ratio = (speed - velocityLimits.x) / (velocityLimits.y - velocityLimits.x);

            float xScale = xDeformLimits.x - (xDeformLimits.y - xDeformLimits.x) * ratio;
            float yScale = yDeformLimits.x + (yDeformLimits.y - yDeformLimits.x) * ratio;

            xScale = Mathf.Lerp(transform.localScale.x, xScale, 0.1f);
            yScale = Mathf.Lerp(transform.localScale.y, yScale, 0.1f);

            transform.localScale = new Vector3(xScale, yScale, 1);
        }
    }
}
