using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorRemover : MonoBehaviour
{
    void Start()
    {
        //Invoke("CursorChange", 1f);
        Cursor.visible = false;
    }
}
