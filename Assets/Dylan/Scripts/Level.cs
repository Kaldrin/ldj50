using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] GameObject cinemachineBrain;
    [SerializeField] Level nextLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLevel()
    {
        EndCurrentLevel();
        nextLevel?.StartLevel();
    }

    void EndCurrentLevel()
    {
        cinemachineBrain.SetActive(false);
    }

    public void StartLevel()
    {
        cinemachineBrain.SetActive(true);
        // Reset Flame State
    }
}
