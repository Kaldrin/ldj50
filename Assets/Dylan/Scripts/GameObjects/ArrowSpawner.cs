using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour, ILevelStart
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

    public void LevelStart()
    {
        if (triggeredAtStart)
        {
            isActive = true;
            Invoke("Shot", timeBeforeStart);
        }
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

    void Shot()
    {
        if (isActive)
        {
            GameObject newArrow = Instantiate(arrowPrefab, transform.position + transform.right, transform.rotation, transform);
            newArrow.GetComponent<Arrow>().Initialize(shotSpeed, lifeTime);
            currentTime = 0;
        }
    }

    public void Deactivate()
    {
        isActive = false;
    }
}
