using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeed : MonoBehaviour
{
    Vector3 startPoint;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("GetStartLocation", 5);
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.D))
        {
            startPoint = transform.localPosition;
            Invoke("PrintDistance", 1);
        }*/
    }

    void GetStartLocation()
    {
        startPoint = transform.localPosition;
            Invoke("PrintDistance", 1);
    }

    void PrintDistance()
    {
        Debug.Log(Vector2.Distance(transform.localPosition, startPoint));
        Debug.Break();
    }
}
