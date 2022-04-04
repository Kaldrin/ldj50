using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraConfinerManager : MonoBehaviour
{
    [SerializeField] GameObject initialConfiner;
    [SerializeField] List<GameObject> allTheOtherConfiners;
    GameObject currentConfiner;

    public void Reset()
    {
        foreach(GameObject confiner in allTheOtherConfiners)
        {
            confiner.SetActive(false);
        }
        initialConfiner.SetActive(true);
        GetComponent<CinemachineConfiner>().m_BoundingShape2D = initialConfiner.GetComponent<Collider2D>();
        currentConfiner = initialConfiner;
    }

    private void Start() {
        currentConfiner = initialConfiner;
    }

    public void ChangeConfiner(GameObject newConfiner)
    {
        newConfiner.SetActive(true);
        GetComponent<CinemachineConfiner>().m_BoundingShape2D = newConfiner.GetComponent<Collider2D>();
        currentConfiner?.SetActive(false);
        currentConfiner = newConfiner;
    }
}
