using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public bool isActive = false;
    [SerializeField] float shotSpeed;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] bool repeat;
    [SerializeField] float timeBeetweenTwoShots;
    [SerializeField] bool triggeredAtStart;
    [SerializeField] float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        if (triggeredAtStart)
            Trigger();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Trigger()
    {
        isActive = true;
        if (repeat) InvokeRepeating("Shot", 0, timeBeetweenTwoShots);
        else Invoke("Shot", 0);
    }

    void Shot()
    {
        if (isActive)
        {
            GameObject newArrow = Instantiate(arrowPrefab, transform.position + transform.right, transform.rotation, transform);
            newArrow.GetComponent<Arrow>().Initialize(shotSpeed, lifeTime);
        }
    }

    public void Deactivate()
    {
        isActive = false;
    }
}
