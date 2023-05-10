using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public bool isActive = false;
    [SerializeField] float shotSpeed;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] bool repeat;
    [SerializeField] float timeBetweenTwoShots;
    [SerializeField] bool triggeredAtStart;
    [SerializeField] float lifeTime;
    [SerializeField] float timeBeforeStart;
    bool pause = false;

    float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        if (triggeredAtStart) Shot();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            if (repeat)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= timeBetweenTwoShots)
                {
                    Shot();
                    currentTime = 0;
                }
            }
        }
    }

    public void Pause()
    {
        pause = true;
    }

    public void Unpause()
    {
        pause = false;
    }

    public void Shot()
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
