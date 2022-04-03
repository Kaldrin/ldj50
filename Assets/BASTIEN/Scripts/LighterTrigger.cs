using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterTrigger : MonoBehaviour
{
    [SerializeField] private Meche ropeToLight = null;
    [SerializeField] private Transform lightingArea = null;
    [SerializeField] private GameObject firePrefab = null;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            LightRopeAtArea();
    }

    void LightRopeAtArea()
    {
        CheckForAdjacentSection();
        Destroy(GetComponent<Collider2D>());
    }
    
    
    void CheckForAdjacentSection()
    {
        for (int i = 0; i < ropeToLight.lineRenderer.positionCount - 1; i++)
        {
            Vector3 position = ropeToLight.lineRenderer.GetPosition(i);
            if (position.z <= 0.2f && Vector3.Distance(position, lightingArea.position) < 2)
            {
                SpreadFire(i);
                return;
            }
        }
    }
    void SpreadFire(int index)
    {
        Flame newFlame = Instantiate(firePrefab).GetComponent<Flame>();
        newFlame.standing = false;
        newFlame.lineRendererToFollow = ropeToLight.lineRenderer;
        newFlame.RestartMovingFromBeginning(index);
    }
}
